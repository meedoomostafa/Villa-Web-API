using VillaWebApi.Data;
using VillaWebApi.Repository.Interfaces;

namespace VillaWebApi.Repository;

public class UnitOfWork  : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IVillaRepository Villa { get; private set;}
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Villa = new VillaRepository(_context);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
