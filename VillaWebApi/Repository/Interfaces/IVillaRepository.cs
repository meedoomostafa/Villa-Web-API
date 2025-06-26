using System.Linq.Expressions;
using VillaWebApi.Models;
namespace VillaWebApi.Repository.Interfaces;

public interface IVillaRepository : IRepository<Villa>
{
    Task<Villa> UpdateAsync(Villa entity);
}
