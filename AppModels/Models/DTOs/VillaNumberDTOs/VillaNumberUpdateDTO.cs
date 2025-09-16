using System.ComponentModel.DataAnnotations;

namespace VillaModels.Models.DTOs.VillaNumberDTOs;

public class VillaNumberUpdateDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int VillaId { get; set; }
    public string SpecialDetails { get; set; }
}
