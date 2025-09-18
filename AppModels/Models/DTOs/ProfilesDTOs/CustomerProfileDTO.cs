using System.ComponentModel.DataAnnotations;

namespace AppModels.Models.DTOs.ProfilesDTOs;

public sealed class CustomerProfileDTO
{
    public int ApplicationUserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    [Phone]
    public string PhoneNumber { get; set; }
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
}