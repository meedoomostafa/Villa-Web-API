using VillaModels.Models;
using VillaRepository.Data;
using VillaRepository.Repository.Interfaces;

namespace VillaRepository.Repository;

public class VillaNumberRepository : Repository<VillaNumber> , IVillaNumberRepository
{
    private readonly ApplicationDbContext _context;
    public VillaNumberRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
    {
        entity.UpdatedDate = DateTime.Now;
        _context.VillaNumbers.Update(entity);
        return entity;
    }
}
