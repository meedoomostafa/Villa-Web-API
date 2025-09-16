using VillaModels.Models;

namespace AppRepository.Repository.Interfaces;

public interface IVillaNumberRepository : IRepository<VillaNumber>
{
    Task<VillaNumber> UpdateAsync(VillaNumber entity);
}
