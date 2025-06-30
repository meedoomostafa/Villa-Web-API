using System.ComponentModel.DataAnnotations;

namespace VillaWeb.Models.DTOs.VillaNumberDTOs;

public class VillaNumberCreateDTO
{
    [Required]
    public int VillaNo { get; set; }
    [Required]
    public int VillaId { get; set; }
    public string SpetialDeatils { get; set; }
}
