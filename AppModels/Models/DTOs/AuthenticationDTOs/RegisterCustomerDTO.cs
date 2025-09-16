using System.ComponentModel.DataAnnotations;

namespace VillaModels.Models.DTOs.AuthenticationDTOs;

public sealed class RegisterCustomerDTO
{
    [StringLength(30, MinimumLength = 3)]
    public string UserName { get; set; }
    
    public string FullName { get; set; }

    [EmailAddress] 
    public string Email { get; set; }


    [DataType(DataType.Password)]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
    
    [Phone]
    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }
    
    public DateTime? BirthOfDate { get; set; }
}
