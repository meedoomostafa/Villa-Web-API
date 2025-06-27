using System.ComponentModel.DataAnnotations;

namespace VillaWebApi.Models.DTOs.VillaNumberDTOs;

public class VillaNumberDTO
{
    [Required]
    public int VillaNo { get; set; }
    public string SpetialDeatils { get; set; }
}
