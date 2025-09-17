
namespace AppModels.Models;

public sealed class Company 
{
    public int ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    
    public string CompanyName { get; set; }
    public string CommercialRegistrationDocUrl { get; set; }
    public bool IsApproved { get; set; } = false;
    public string Country { get; set; }
    public string City { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Villa>? Villas { get; set; }
}