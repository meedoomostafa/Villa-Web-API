using System.ComponentModel.DataAnnotations;
using AppModels.Models.DTOs.VillaDTOs;

namespace AppModels.Models.DTOs.VillaNumberDTOs;

public class VillaNumberDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int VillaId { get; set; }
    public string SpecialDetails { get; set; }
    public VillaDTO Villa { get; set; }
}
