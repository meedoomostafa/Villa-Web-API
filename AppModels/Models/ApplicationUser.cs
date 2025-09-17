using Microsoft.AspNetCore.Identity;

namespace AppModels.Models;

public sealed class ApplicationUser : IdentityUser<int>
{
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}
