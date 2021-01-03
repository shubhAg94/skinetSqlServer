using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        //You will use configure services to add methods to the service container so that you can use 
        //them using dependency injection in your project
        public void ConfigureServices(IServiceCollection services)
        {
            //AddTransient(): This one is instantiated for a individual method and not the
            //request itself. So it's got a very short lifetime the repository will be created and destroyed just upon using an 
            //individual method.

            //AddSingleton(): If we use this option the repository would be created the first time we use it when the 
            //application starts and the method goes to the controller and creates a new instance of the repository but 
            //then it would never be destroyed until the application shuts down and that's too long.

            //AddScoped(): So the correct one is the AddScoped option and this is the one that we'll use for almost everything.
            //So it's gonna be created when the TTP request comes into our API creates a new instance in the controller
            //the controller sees that it needs a repository so it creates the instance of the repository.
            //And when the request is finished then it disposes of both the controller and the repository.
            //And we don't ourselves need to worry about disposing of the resources created when a request comes in.
            //So in order to add our repository we will use AddScoped() and we'll pass in the IProductRepository.
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddControllers();
            services.AddDbContext<StoreContext>(x =>
            x.UseSqlServer(_config.GetConnectionString("DefaultConnection")));

            //Completely separate database for Identity
            //t's going to be a physical contact boundary between our own application database
            services.AddDbContext<AppIdentityDbContext>(x => {
                x.UseSqlServer(_config.GetConnectionString("IdentityConnection"));
            });

            //Redius connection is designed to be shared and reused between callers and 
            //is fully thread safe and ready for less particular usage.
            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            services.AddApplicationServices();
            services.AddIdentityServices(_config);
            services.AddSwaggerDocumentation();

            //Cors is a mechanism that's used to tell browsers to give a web application running 
            //at one origin access to selected resources from a different origin.
            //For security reasons browsers restrict cross origin HTTP requests initiated from javascript.
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy => 
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        //The configuring method is used to configure the HTTP pipelines so any middlewares 
        //that you want to add. We will be adding that inside the configure method.

        /*Whenever an HTTP request comes in something must handle that request. So it eventually results in 
        an HTTP response. So those pieces of code that handles the request and results in a response 
        make up the request pipeline.

        Middleware: What we can do is, configure this request pipeline by adding middlewares which are 
        software components that are assembled into an application pipeline to handle request and response.

        Now keep in mind the order of pipeline is important. It always gets passed from the first to the last.
        A good example will be authentication middleware.

        If the middleware component finds that a request isn't authorized it will not pass it to the next piece
        of the middleware but it will immediately return an unauthorized response.
        It is hence important that authentication Middleware is added before other components that shouldn't
        be triggered in case of an unauthorized request.
         */
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            //If we call api with http then, firstwe get a 307(temporary redirect) response
            //later we get 200 response from htts assecond response and that is actual response fro api
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            //I like to do this after the UseHttpsRedirection as that ensures that any call to a non encrypted
            //open API endpoint will be redirected to encrypted version.
            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
