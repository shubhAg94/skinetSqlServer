using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        // SymmetricSecurityKey is a type of encryption where only one key "a secret key" which
        // we're going to store on our server is used to both encrypt and decrypt our signature in the token.
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:key"]));
        }

        public string CreateToken(AppUser appUser)
        {
            /*
             Now a claim is a bit of information about the user. it could be like a user might have a date of birth
             and they would claim like date of birth is this or a user has an email and they're claiming their email is this.
             And that's what we're talking about with claims in this respect.
             */
            var claims = new List<Claim>
            {
                /*
                 Now these claims inside our token are going to be able to be decoded by the client.
                So if a user has token they'll be able to look inside the token and they'll be able to see their
                properties follow email and the display name. If they wanted to do so and that's fine.
                So we just need to be careful that we don't put any sensitive information that we wouldn't want the user
                to see inside there.
                 */
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, appUser.DisplayName),
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();  
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);   
        }
    }
}
