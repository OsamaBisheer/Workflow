using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Workflow.API.JWT
{
    public class RevokableJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        private readonly IConfiguration _configuration;

        public RevokableJwtSecurityTokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Issuer"],
            };

            var claimsPrincipal = base.ValidateToken(token, tokenValidationParameters, out validatedToken);

            return claimsPrincipal;
        }
    }
}