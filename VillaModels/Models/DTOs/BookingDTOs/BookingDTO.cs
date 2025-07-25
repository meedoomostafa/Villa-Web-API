using VillaWebApiUtilities;

namespace VillaModels.Models.DTOs.BookingDTOs;

public class BookingDTO
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal TotalPrice { get; set; }
    public SD.BookingStatus Status { get; set; }

    public int ApplicationUserId { get; set; }
    public string UserFullName { get; set; }

    public int VillaNumberId { get; set; }
    public string VillaSpecialDetails { get; set; }

    public string VillaName { get; set; }
    public double VillaRate { get; set; }
    public string VillaImageUrl { get; set; }
}