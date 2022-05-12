using Microsoft.AspNetCore.Identity;

namespace Budget.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? Name { get; internal set; }
    }   
}
