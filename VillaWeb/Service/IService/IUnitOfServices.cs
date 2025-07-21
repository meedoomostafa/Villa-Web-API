namespace VillaWeb.Service.IService;

public interface IUnitOfServices
{
    IVillaService VillaService { get; }
    IVillaNumberService VillaNumberService { get; }
    IAuthenticationService AuthenticationService { get; }
}
