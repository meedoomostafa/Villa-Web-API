using AppWebApiUtilities;

namespace AppModels.Models;

public sealed class Booking
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending; // Default
    
    //foreign keys
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int VillaNumberId { get; set; }
    public VillaNumber VillaNumber { get; set; }
}
