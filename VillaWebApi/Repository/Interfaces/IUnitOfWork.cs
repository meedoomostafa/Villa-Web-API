namespace VillaWebApi.Repository.Interfaces;

public interface IUnitOfWork
{
    public IVillaRepository Villa { get; }
    public IVillaNumberRepository VillaNumber { get; }
    Task SaveChangesAsync();
}
