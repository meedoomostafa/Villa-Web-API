using System.ComponentModel.DataAnnotations;
using VillaModels.Models.DTOs.VillaDTOs;

namespace VillaModels.Models.DTOs.VillaNumberDTOs;

public class VillaNumberDTO
{
    [Required]
    public int VillaNo { get; set; }
    [Required]
    public int VillaId { get; set; }
    public string SpetialDeatils { get; set; }
    public VillaDTO Villa { get; set; }
}
