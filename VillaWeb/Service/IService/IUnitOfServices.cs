namespace VillaWeb.Service.IService;

public interface IUnitOfServices
{
    IVillaService VillaService { get; }
    IVillaNumberService VillaNumberService { get; }
    IBookingService BookingService { get; }
    IAuthenticationService AuthenticationService { get; }
}
