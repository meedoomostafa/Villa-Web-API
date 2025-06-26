namespace VillaModels.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime Created { get; set; } 
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public string? JwtTokenId { get; set; } 
    public int UserId { get; set; }
    public ApplicationUser User { get; set; }
}