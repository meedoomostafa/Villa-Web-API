using AppService.Helpers;
using Microsoft.Extensions.DependencyInjection;
using AppService.Implementations;
using AppService.Interfaces;

namespace AppService.Extenstions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IVillaService, VillaService>();
        services.AddScoped<IVillaNumberService, VillaNumberService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IUnitOfServices, UnitOfServices>();
        
        services.AddScoped<AccountHelper>();
        return services;
    } 
}