namespace VillaModels.Models.DTOs.AdminManagementDTOs;

public class PendingCompanyDTO
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string Email { get; set; }
    public DateTime RegisteredAt { get; set; }
    public bool IsApproved { get; set; }
    public string? CommercialRegistrationDocUrl { get; set; } 
}