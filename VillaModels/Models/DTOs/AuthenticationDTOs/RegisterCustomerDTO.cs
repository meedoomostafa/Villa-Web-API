using System.ComponentModel.DataAnnotations;

namespace VillaModels.Models.DTOs.AuthenticationDTOs;

public sealed class RegisterCustomerDTO
{
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string UserName { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    public string? LastName { get; set; }

    [Required]
    [EmailAddress] 
    public string Email { get; set; }


    [Required]
    [DataType(DataType.Password)]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
    
    [Required]
    [Phone]
    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }
    
    public string Role { get; set; }
    
    public DateTime? BirthOfDate { get; set; }
}
