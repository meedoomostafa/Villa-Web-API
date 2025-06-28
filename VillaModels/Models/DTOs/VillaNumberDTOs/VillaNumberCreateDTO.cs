using System.ComponentModel.DataAnnotations;

namespace VillaModels.Models.DTOs.VillaNumberDTOs;

public class VillaNumberCreateDTO
{
    [Required]
    public int VillaNo { get; set; }
    [Required]
    public int VillaId { get; set; }
    public string SpetialDeatils { get; set; }
}
