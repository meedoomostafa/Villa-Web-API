using VillaModels.Models;

namespace AppService.Interfaces;

public interface IRefreshTokenService
{
    Task CreateRefreshTokenAsync(RefreshToken refreshToken);
    Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
    Task<RefreshToken> GetRefreshTokenWithDeviceIdNotRevokedAsync(string deviceId);
    Task<IEnumerable<RefreshToken>> GetAllRefreshTokensForDeviceIdNotRevokedAsync(string deviceId);
    Task<IEnumerable<RefreshToken>> GetAllRefreshTokensForUserDeviceNotRevokedAsync(int userId, string deviceId);
    Task<IEnumerable<RefreshToken>> GetAllUserTokensByUserIdNotRevokedAsync(int userId);
}