using Microsoft.AspNetCore.Identity;

namespace VillaModels.Models;

public sealed class ApplicationUser : IdentityUser<int>
{
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}
