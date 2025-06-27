using System.ComponentModel.DataAnnotations;

namespace VillaWebApi.Models.DTOs.VillaNumberDTOs;

public class VillaNumberCreateDTO
{
    [Required]
    public int VillaNo { get; set; }
    public string SpetialDeatils { get; set; }
}
