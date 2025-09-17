namespace AppModels.Models;
public sealed class Villa
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Details { get; set; }
    public decimal Price { get; set; }
    public int Sqft { get; set; }
    public int Occupancy { get; set; }
    public string ImageUrl { get; set; }
    public string Amenity { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string LocationAddress { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; }

    public ICollection<VillaNumber> VillaNumbers { get; set; } = new List<VillaNumber>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
