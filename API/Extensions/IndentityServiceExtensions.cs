using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class IndentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            var builder = services.AddIdentityCore<AppUser>();

            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<AppIdentityDbContext>();
            builder.AddSignInManager<SignInManager<AppUser>>();

            //Here we're going to tell AddAuthentication, what type of authentication we're using and how it needs to 
            //validate the token that we're going to pass to our clients.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //if we forget to add this then we might as well just leave anonymous authentication on and a user can
                        //send up any old token they want because we would never validate that the signing key is correct.
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:key"])),
                        ValidIssuer = config["Token:issuer"],
                        //validates the issuer otherwise again it will just accepts any issuer of any token 
                        //so we'll set that to true as well.
                        ValidateIssuer = true,
                        //Audience who the token was issued to. we're only going to accept tokens from this particular
                        //audience and our audience would be our client application.
                        ValidateAudience = false
                    };
                });

            return services;
        }
    }
}
