using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Carpediem.Utils
{
    public class TokenService
    {
        private readonly IConfiguration Configuration;

        public TokenService(IConfiguration configuration)
        {
            Configuration = configuration.GetSection("Jwt");
        }

        public string GenerateToken(int id, string role, string name)
        {
            var claims = new Claim[] {
                new Claim("Id", id.ToString()),
                new Claim("Role", role),
                new Claim("Name", name)
            };
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Key"))), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                null,
                null,
                claims,
                null,
                DateTime.Now.AddHours(1),
                signingCredentials
            );
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }

    public static class TokenServiceExtensions
    {
        public static AuthenticationBuilder AddToken(this IServiceCollection services, IConfiguration Configuration)
        {
            return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Jwt:Key"))),
                    RoleClaimType = "Role"
                };
            });
        }
    }
}