using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using swf.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace swf.Libraries
{
    public static class GenerateJwtToken
    {
        public static string ReturnJwtToken(IdentityUser user, IConfiguration _configuration)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key=Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
            //create a token descriptor , a token has 3 parts header, body, signature
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),
                Expires=DateTime.Now.AddHours(1),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
               
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;

        }
    }
}