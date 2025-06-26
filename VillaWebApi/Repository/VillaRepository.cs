using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VillaWebApi.Data;
using VillaWebApi.Models;
using VillaWebApi.Repository.Interfaces;

namespace VillaWebApi.Repository;

public class VillaRepository : Repository<Villa> , IVillaRepository
{
    private ApplicationDbContext _context;
    public VillaRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Villa> UpdateAsync(Villa entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.Villas.Update(entity);
        return entity;
    }
}
