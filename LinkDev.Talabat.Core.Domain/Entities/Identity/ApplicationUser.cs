using Microsoft.AspNetCore.Identity;

namespace LinkDev.Talabat.Core.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public required string DisplayName { get; set; }

        public virtual Address? Address { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}
