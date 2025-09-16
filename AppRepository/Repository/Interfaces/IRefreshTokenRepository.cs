using VillaModels.Models;

namespace AppRepository.Repository.Interfaces;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task UpdateAsync(RefreshToken entity);
}