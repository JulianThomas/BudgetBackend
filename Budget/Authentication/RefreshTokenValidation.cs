using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;   
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Budget.Authentication
{
    public class RefreshTokenValidation:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        readonly JwtSecurityTokenHandler _tokenHandler = new();
        private ClaimsPrincipal? principal;
        public RefreshTokenValidation(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> GetUserWithToken(string refreshToken)
        {
            try
            {
                principal = _tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSecretKey"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
            }
            catch (Exception)
            {
                return null;
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var username = principal.Identity.Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            var user = await _userManager.FindByNameAsync(username);
            return user;
        }
    }
}

