using System.Linq.Expressions;

namespace VillaRepository.Repository.Interfaces;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null , Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task<T> GetAsync(Expression<Func<T, bool>>? filter = null ,Func<IQueryable<T>, IQueryable<T>>? include = null , bool tracked = true);
    Task CreateAsync(T entity);
    Task RemoveAsync(T entity);
}
