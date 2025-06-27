using System.ComponentModel.DataAnnotations;

namespace VillaWebApi.Models.DTOs.VillaNumberDTOs;

public class VillaNumberUpdateDTO
{
    [Required]
    public int VillaNo { get; set; }
    public string SpetialDeatils { get; set; }
}
