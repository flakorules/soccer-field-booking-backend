using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SoccerFieldBooking.API.Abstractions.Helpers;
using SoccerFieldBooking.API.Config;
using SoccerFieldBooking.API.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.Helpers
{
    public class JwtHelper:IJwtHelper
    {
        private readonly JwtConfig _jwtConfig;
        public JwtHelper(IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value;
        }

        public string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_jwtConfig.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public int GetUserIdFromBearerToken(string bearerToken)
        {
            var tokenArray = bearerToken.Split(' ');
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenArray[1]);
            return int.Parse(token.Claims.First(c => c.Type == "UserId").Value);
        }

        public string GetBearerToken(HttpRequest request)
        {
            return request.Headers["Authorization"].ToString();
        }
    }
}
