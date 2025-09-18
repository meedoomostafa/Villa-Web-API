using AppService.MappingProfiles;

namespace VillaWebApi.Extensions;

public static class MappingConfigExtension
{
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AuthenticationMapping));
        services.AddAutoMapper(typeof(VillaMapping));
        services.AddAutoMapper(typeof(VillaNumberMapping));
        services.AddAutoMapper(typeof(BookingMapping));
        services.AddAutoMapper(typeof(CustomerMapping));
        services.AddAutoMapper(typeof(CompanyMapping));
        return services;
    }
}