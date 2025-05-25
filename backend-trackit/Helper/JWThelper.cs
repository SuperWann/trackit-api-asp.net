using backend_trackit.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

namespace backend_trackit.Helper
{
    public class JWThelper
    {
        private readonly IConfiguration _configuration;

        public JWThelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string generateTokenPegawai(Pegawai pegawai)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, pegawai.id.ToString()),
                    new Claim("Id", pegawai.id.ToString()),
                    new Claim(ClaimTypes.Email, pegawai.email),
                    new Claim(ClaimTypes.Role, pegawai.role),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
