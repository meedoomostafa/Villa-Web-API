using System.ComponentModel.DataAnnotations;

namespace VillaModels.Models.DTOs.VillaDTOs;

public sealed class VillaCreateDTO
{
    public int CompanyId { get; set; }
    public string Name { get; set; }
    public string Details { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Sqft { get; set; }
    [Required]
    public int Occupancy { get; set; }
    public string ImageUrl { get; set; }
    public string Amenity { get; set; }
    [Required]
    public double Latitude { get; set; }
    [Required]
    public double Longitude { get; set; }
    public string LocationAddress { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
}
