namespace AppRepository.Repository.Interfaces;

public interface IUnitOfWork
{
    public IVillaRepository Villa { get; }
    public IVillaNumberRepository VillaNumber { get; }
    public IBookingRepository Booking { get; }
    public ICustomerRepository Customer { get; }
    public ICompanyRepository Company { get; }
    public IReviewRepository Review { get; }
    public IRefreshTokenRepository RefreshTokens { get; }
    Task SaveChangesAsync();
}
