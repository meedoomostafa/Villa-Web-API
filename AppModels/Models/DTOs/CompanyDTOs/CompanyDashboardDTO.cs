using AppModels.Models.DTOs.BookingDTOs;
using AppModels.Models.DTOs.VillaDTOs;

namespace AppModels.Models.DTOs.CompanyDTOs;

public sealed class CompanyDashboardDTO
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string Country { get; set; }
    public string City { get; set; }

    public List<VillaDashboardDTO> Villas { get; set; } = new();
}