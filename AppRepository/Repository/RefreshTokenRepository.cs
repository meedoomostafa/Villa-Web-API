using AppRepository.Data;
using AppRepository.Repository.Interfaces;
using VillaModels.Models;

namespace AppRepository.Repository;

public class RefreshTokenRepository : Repository<RefreshToken> , IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task UpdateAsync(RefreshToken entity)
    {
        _context.RefreshTokens.Update(entity);
        return Task.CompletedTask;
    }
}