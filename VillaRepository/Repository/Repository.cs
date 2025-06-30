using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VillaRepository.Data;
using VillaRepository.Repository.Interfaces;

namespace VillaRepository.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    internal DbSet<T> DbSet;
    
    public Repository(ApplicationDbContext context)
    {
        _context = context;
        this.DbSet = _context.Set<T>();
    }
    
    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null , Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        IQueryable<T> query = DbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (include != null)
        {
            query = include(query);
        }
        return await query.ToListAsync();
    }

    

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IQueryable<T>>? include = null ,bool tracked = true )
    {
        IQueryable<T> query = DbSet;
        if (!tracked)
        {
            query = query.AsNoTracking();
        }
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (include != null)
        {
            query = include(query);
        }
        return await query.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task RemoveAsync(T entity)
    {
        DbSet.Remove(entity);
    }
}
