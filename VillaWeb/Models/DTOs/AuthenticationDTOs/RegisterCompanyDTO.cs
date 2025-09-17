using System.ComponentModel.DataAnnotations;

namespace VillaWeb.Models.DTOs.AuthenticationDTOs;

public class RegisterCompanyDTO
{
    [StringLength(30, MinimumLength = 3)]
    public string UserName { get; set; }
    public string CompanyName { get; set; }
    public string CommercialRegistrationDocUrl { get; set; }

    [EmailAddress] 
    public string Email { get; set; }


    [DataType(DataType.Password)]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
    
    [Phone]
    public string PhoneNumber { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}