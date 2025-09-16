using AppService.Interfaces;
using AppRepository.Repository.Interfaces;

namespace AppService.Implementations;

public class UnitOfServices : IUnitOfServices
{
    public IUnitOfWork _unitOfWork { get; set; }
    public IAccountService Account { get; private set; }
    public IVillaService Villas { get; private set; }
    public IBookingService Bookings { get; private set; }
    public ICustomerService Customers { get; private set; }
    public ICompanyService Companies { get; private set; }
    public IAdminService Admins { get; private set; }
    public IVillaNumberService VillaNumbers { get; private set; }
    public IRefreshTokenService RefreshTokens { get; private set; }

    public UnitOfServices(IUnitOfWork unitOfWork,
        IVillaService villas,
        IBookingService bookings,
        ICustomerService customers,
        ICompanyService companies,
        IAdminService admins,
        IVillaNumberService villaNumbers,
        IRefreshTokenService refreshTokenService,
        IAccountService account)
    {
        Villas = villas;
        Bookings = bookings;
        Customers = customers;
        Companies = companies;
        Admins = admins;
        VillaNumbers = villaNumbers;
        RefreshTokens = refreshTokenService;
        Account = account;
    }
    
    public async Task SaveChangesAsync()
    {
        await _unitOfWork.SaveChangesAsync();
    }
}