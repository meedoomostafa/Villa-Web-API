
namespace VillaModels.Models;

public class VillaNumber
{
    public int VillaNo { get; set; }
    public int? VillaId { get; set; }
    public Villa Villa { get; set; }
    public string SpetialDeatils { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
