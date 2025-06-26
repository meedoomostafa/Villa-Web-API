namespace VillaModels.Models.DTOs.BookingDTOs;

public class BookingCreateDTO
{
    public int ApplicationUserId { get; set; }
    public int VillaNumberId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
}