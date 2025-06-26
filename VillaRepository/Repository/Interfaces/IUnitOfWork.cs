namespace VillaRepository.Repository.Interfaces;

public interface IUnitOfWork
{
    public IVillaRepository Villa { get; }
    public IVillaNumberRepository VillaNumber { get; }
    public IBookingRepository Booking { get; }
    public IRefreshTokenRepository RefreshTokens { get; }
    Task SaveChangesAsync();
}
