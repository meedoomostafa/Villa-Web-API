namespace VillaWeb.Models.DTOs.CompanyDTOs;

public sealed class VillaDashboardDTO
{
    public int VillaId { get; set; }
    public string Name { get; set; }
    public string Details { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public string City { get; set; }

    public List<VillaNumberDashboardDTO> VillaNumbers { get; set; } = new();
}