using AppWebApiUtilities;

namespace AppModels.Models.DTOs.CompanyDTOs;

public class BookingDashboardDTO
{
    public int BookingId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }

    public CustomerDashboardDTO Customer { get; set; }
}