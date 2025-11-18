namespace VillaWeb.Models.DTOs.CompanyDTOs;

public sealed class VillaNumberDashboardDTO
{
    public int VillaNumberId { get; set; }
    public string SpecialDetails { get; set; }

    public List<BookingDashboardDTO> Bookings { get; set; } = new();
}