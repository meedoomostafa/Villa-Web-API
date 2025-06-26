using VillaModels.Models;

namespace VillaRepository.Repository.Interfaces;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task UpdateAsync(RefreshToken entity);
}