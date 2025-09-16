
using System.ComponentModel.DataAnnotations.Schema;

namespace VillaModels.Models;

public sealed class VillaNumber
{
    public int Id { get; set; }
    public string SpecialDetails { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int VillaId { get; set; }
    public Villa Villa { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
