using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace DormPaymentSystem.API.Infrastructure
{
    public class TokenService : ITokenService
    {

        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(User user, IList<string> roles)
        {
            //  get the configuration values for JWT

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]!);

            // claims for the token 

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id),
                 new Claim(ClaimTypes.Email, user.Email!),
                 new Claim(ClaimTypes.Name, user.FullName),
            };

            // add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Combine everything into one token object
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            // Convert the token object to the encoded string the frontend uses
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
