using AppRepository.Data;
using AppRepository.Repository.Interfaces;

namespace AppRepository.Repository;

public class UnitOfWork  : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IVillaRepository Villa { get; private set;}
    public IVillaNumberRepository VillaNumber { get; private set;}
    public IBookingRepository Booking { get; private set; }
    public ICustomerRepository Customer { get; private set; }
    public ICompanyRepository Company { get; private set; }
    public IReviewRepository Review { get; private set; }
    public IRefreshTokenRepository RefreshTokens { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Villa = new VillaRepository(_context);
        VillaNumber = new VillaNumberRepository(_context);
        Booking = new BookingRepository(_context);
        Customer = new CustomerRepository(_context);
        Company = new CompanyRepository(_context);
        Review = new ReviewRepository(_context);
        RefreshTokens = new RefreshTokenRepository(_context);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
