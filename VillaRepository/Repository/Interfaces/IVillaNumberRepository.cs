using VillaModels.Models;

namespace VillaRepository.Repository.Interfaces;

public interface IVillaNumberRepository : IRepository<VillaNumber>
{
    Task<VillaNumber> UpdateAsync(VillaNumber entity);
}
