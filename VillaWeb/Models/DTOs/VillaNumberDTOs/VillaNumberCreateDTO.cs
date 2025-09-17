using System.ComponentModel.DataAnnotations;

namespace VillaWeb.Models.DTOs.VillaNumberDTOs;

public class VillaNumberCreateDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int VillaId { get; set; }
    public string SpecialDetails { get; set; }
}
