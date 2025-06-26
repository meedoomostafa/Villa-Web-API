using System.ComponentModel.DataAnnotations;

namespace VillaWeb.Models.DTOs.AuthenticationDTOs;

public class LoginDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
