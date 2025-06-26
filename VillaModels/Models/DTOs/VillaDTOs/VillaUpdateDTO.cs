using System.ComponentModel.DataAnnotations;

namespace VillaModels.Models.DTOs.VillaDTOs;

public sealed class VillaUpdateDTO
{
    [Required] 
    public int Id { get; set; }

    [Required] [StringLength(30)] 
    public string Name { get; set; }

    [Required] 
    public string Details { get; set; }

    [Required] 
    public double Rate { get; set; }

    [Required] 
    public int Occupancy { get; set; }

    [Required] 
    public int Sqft { get; set; }

    [Required] 
    public string ImageUrl { get; set; }

    public string Amenity { get; set; }
}
