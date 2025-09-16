using AppRepository.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppRepository.Repository.Extensions;

public static class RepositoryCollectionExtensions
{
    public static IServiceCollection AddUnitOfWorkRepository(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}