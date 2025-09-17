namespace AppModels.Models;

public class Customer
{
    public int ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}