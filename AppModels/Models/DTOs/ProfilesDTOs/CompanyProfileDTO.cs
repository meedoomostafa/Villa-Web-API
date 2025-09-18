using System.ComponentModel.DataAnnotations;

namespace AppModels.Models.DTOs.ProfilesDTOs;

public sealed class CompanyProfileDTO
{
    public int ApplicationUserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    [Phone]
    public string PhoneNumber { get; set; }
    public string CompanyName { get; set; }
    public string CommercialRegistrationDocUrl { get; set; }
    public bool IsApproved { get; set; } = false;
    public string Country { get; set; }
    public string City { get; set; }
    public DateTime CreatedAt { get; set; }    
}