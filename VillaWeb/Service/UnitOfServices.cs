using VillaWeb.Service.IService;

namespace VillaWeb.Service;

public class UnitOfServices : IUnitOfServices
{
    private readonly IHttpClientFactory _httpClient;
    
    public IVillaService VillaService { get; private set; }
    public IVillaNumberService VillaNumberService { get; private set; }
    public IBookingService BookingService { get; private set; }
    public IAuthenticationService AuthenticationService { get; private set; }
    public ICustomerService CustomerService { get; private set; }
    public ICompanyService CompanyService { get; private set; }


    public UnitOfServices(
        IHttpClientFactory httpClient , 
        IConfiguration configuration , 
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        VillaService = new VillaService(httpClient,configuration,httpContextAccessor);
        VillaNumberService = new VillaNumberService(httpClient,configuration,httpContextAccessor);
        BookingService = new BookingService(httpClient,configuration,httpContextAccessor);
        AuthenticationService = new AuthenticationService(httpClient,configuration,httpContextAccessor);
        CustomerService = new CustomerService(httpClient,configuration,httpContextAccessor);
        CompanyService = new CompanyService(httpClient,configuration,httpContextAccessor);
    }
}
