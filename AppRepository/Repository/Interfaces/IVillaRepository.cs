using VillaModels.Models;
namespace AppRepository.Repository.Interfaces;

public interface IVillaRepository : IRepository<Villa>
{
    Task UpdateAsync(Villa entity);
}
