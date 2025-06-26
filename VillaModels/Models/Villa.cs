namespace VillaModels.Models;
public sealed class Villa
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Details { get; set; }
    public double rate { get; set; }
    public int Sqft { get; set; }
    public int Occupancy { get; set; }
    public string ImageUrl { get; set; }
    public string Amenity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public List<VillaNumber> VillaNumbers { get; set; }
}
