using System.Linq.Expressions;
using AppRepository.Data;
using AppRepository.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using AppModels.Models;

namespace AppRepository.Repository;

public class VillaRepository : Repository<Villa> , IVillaRepository
{
    private ApplicationDbContext _context;
    public VillaRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task UpdateAsync(Villa entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.Villas.Update(entity);
        return Task.CompletedTask;
    }
}
