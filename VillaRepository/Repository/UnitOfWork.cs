using VillaRepository.Data;
using VillaRepository.Repository.Interfaces;

namespace VillaRepository.Repository;

public class UnitOfWork  : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IVillaRepository Villa { get; private set;}
    public IVillaNumberRepository VillaNumber { get; private set;}
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Villa = new VillaRepository(_context);
        VillaNumber = new VillaNumberRepository(_context);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
