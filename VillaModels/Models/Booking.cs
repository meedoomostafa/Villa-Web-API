using VillaWebApiUtilities;

namespace VillaModels.Models;

public sealed class Booking
{
    public int Id { get; set; }
    public int ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public int VillaNumberId { get; set; }
    public VillaNumber VillaNumber { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal TotalPrice { get; set; }

    public SD.BookingStatus Status { get; set; } = SD.BookingStatus.Pending; // Default
}
