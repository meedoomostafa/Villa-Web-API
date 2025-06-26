namespace VillaWebApi.Repository.Interfaces;

public interface IUnitOfWork
{
    public IVillaRepository Villa { get; }
    Task SaveChangesAsync();
}
