using System.ComponentModel.DataAnnotations.Schema;

namespace VillaWebApi.Models;

public class VillaNumber
{
    public int VillaNo { get; set; }
    public string SpetialDeatils { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
