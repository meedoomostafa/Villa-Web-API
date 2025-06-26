using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaWeb.Models.DTOs.VillaNumberDTOs;

namespace VillaWeb.Models.DTOs.VillaDTOs;

public class VillaWithVillaNumbersDTO
{
    public int Id { get; set; }

    [Required] [StringLength(30)] 
    public string Name { get; set; }

    public string Details { get; set; }

    [Required] public double Rate { get; set; }

    public int Occupancy { get; set; }
    public int Sqft { get; set; }
    public string ImageUrl { get; set; }
    public string Amenity { get; set; }
    public ICollection<VillaNumberDTO> VillaNumbers { get; set; }
    public IEnumerable<SelectListItem> VillaNumbersSelectList { get; set; }
}