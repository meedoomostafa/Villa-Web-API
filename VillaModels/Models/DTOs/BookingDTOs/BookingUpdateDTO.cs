using VillaWebApiUtilities;
namespace VillaModels.Models.DTOs.BookingDTOs;

public class BookingUpdateDTO
{
    public int Id { get; set; }
    public int VillaNumberId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public SD.BookingStatus Status { get; set; } 
}