using AppRepository.Data;
using AppRepository.Repository.Interfaces;
using AppModels.Models;

namespace AppRepository.Repository;

public class VillaNumberRepository : Repository<VillaNumber> , IVillaNumberRepository
{
    private readonly ApplicationDbContext _context;
    public VillaNumberRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.VillaNumbers.Update(entity);
        return entity;
    }
}
