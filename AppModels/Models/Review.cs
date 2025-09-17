namespace AppModels.Models;

public class Review
{
    public int Id { get; set; }
    public byte Rating { get; set; }
    public string Comment { get; set; }
    
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    
    public int VillaId { get; set; }
    public Villa  Villa { get; set; }
}