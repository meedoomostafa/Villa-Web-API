
namespace VillaModels.Models;

public sealed class VillaNumber
{
    public int VillaNo { get; set; }
    public int? VillaId { get; set; }
    public Villa Villa { get; set; }
    public int? BookingId { get; set; }
    public ICollection<Booking> Booking { get; set; } = new List<Booking>();
    public string SpetialDeatils { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
