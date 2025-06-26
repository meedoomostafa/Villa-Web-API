using Microsoft.AspNetCore.Identity;

namespace VillaModels.Models;

public sealed class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}
