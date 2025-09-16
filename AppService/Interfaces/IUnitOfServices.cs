using AppService.Implementations;

namespace AppService.Interfaces;

public interface IUnitOfServices
{
    IAccountService Account { get; }
    IVillaService Villas { get; }
    IBookingService Bookings { get; }
    ICustomerService Customers { get; }
    ICompanyService Companies { get; }
    IAdminService Admins { get; }
    IVillaNumberService VillaNumbers { get; }
    IRefreshTokenService RefreshTokens { get; }
    
    Task SaveChangesAsync();
}