namespace VillaModels.Models.DTOs;

public class AdminDashboardDTO
{
    public int TotalCompanies { get; set; }
    public int NumberOfPendingCompanies { get; set; }
    public int ApprovedCompanies { get; set; }

    public int TotalVillas { get; set; }
    public int TotalVillaNumbers { get; set; }
    public int TotalBookings { get; set; }

    public List<PendingCompanyDTO> PendingCompanies { get; set; }
    
}