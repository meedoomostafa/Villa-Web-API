using VillaWebApi.Models;

namespace VillaWebApi.Repository.Interfaces;

public interface IVillaNumberRepository : IRepository<VillaNumber>
{
    Task<VillaNumber> UpdateAsync(VillaNumber entity);
}
