using VillaModels.Models;
namespace VillaRepository.Repository.Interfaces;

public interface IVillaRepository : IRepository<Villa>
{
    Task<Villa> UpdateAsync(Villa entity);
}
