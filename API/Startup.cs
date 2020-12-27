using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddApplicationServices();
            services.AddSwaggerDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            //if we call api with http then, firstwe get a 307(temporary redirect) response
            //later we get 200 response from htts assecond response and that is actual response fro api
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
