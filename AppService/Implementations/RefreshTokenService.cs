using AppService.Interfaces;
using VillaModels.Models;
using AppRepository.Repository.Interfaces;

namespace AppService.Implementations;

public class RefreshTokenService :  IRefreshTokenService
{
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _unitOfWork.RefreshTokens.CreateAsync(refreshToken);
    }

    public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _unitOfWork.RefreshTokens.UpdateAsync(refreshToken);
    }

    public async Task<RefreshToken> GetRefreshTokenWithDeviceIdNotRevokedAsync(string deviceId)
    {
        return await _unitOfWork.RefreshTokens
            .GetAsync(t => t.DeviceId == deviceId && !t.IsRevoked);
    }

    public async Task<IEnumerable<RefreshToken>> GetAllRefreshTokensForDeviceIdNotRevokedAsync(string deviceId)
    {
        return await _unitOfWork.RefreshTokens
            .GetAllAsync(t => t.DeviceId == deviceId && !t.IsRevoked);

    }

    public async Task<IEnumerable<RefreshToken>> GetAllRefreshTokensForUserDeviceNotRevokedAsync(int userId , string deviceId)
    {
        return await _unitOfWork.RefreshTokens.GetAllAsync(t =>
            t.UserId == userId && t.DeviceId == deviceId && !t.IsRevoked);
    }

    public async Task<IEnumerable<RefreshToken>> GetAllUserTokensByUserIdNotRevokedAsync(int userId)
    {
        return await _unitOfWork.RefreshTokens
            .GetAllAsync(t => t.UserId == userId && !t.IsRevoked);
    }
}