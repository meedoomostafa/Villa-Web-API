using System.ComponentModel.DataAnnotations;
using VillaWeb.Models.DTOs.VillaDTOs;

namespace VillaWeb.Models.DTOs.VillaNumberDTOs;

public class VillaNumberDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int VillaId { get; set; }
    public string SpecialDetails { get; set; }
    public VillaDTO Villa { get; set; }
}
