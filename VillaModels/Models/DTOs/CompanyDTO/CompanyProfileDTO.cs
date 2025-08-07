namespace VillaModels.Models.DTOs.AdminManagementDTOs;

public class CompanyProfileDTO
{
    public string CompanyName { get; set; }
    public string Email { get; set; }
    public bool IsApproved { get; set; }
    public string StatusMessage => IsApproved ? "Approved" : "Pending Approval";
}