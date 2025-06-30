using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VillaModels.Models;
using VillaRepository.Data;
using VillaRepository.Repository.Interfaces;

namespace VillaRepository.Repository;

public class VillaRepository : Repository<Villa> , IVillaRepository
{
    private ApplicationDbContext _context;
    public VillaRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(Villa entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.Villas.Update(entity);
    }
}
