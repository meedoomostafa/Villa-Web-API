using VillaModels.Models;
using VillaRepository.Data;
using VillaRepository.Repository.Interfaces;

namespace VillaRepository.Repository;

public class RefreshTokenRepository : Repository<RefreshToken> , IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(RefreshToken entity)
    {
        _context.RefreshTokens.Update(entity);
    }
}