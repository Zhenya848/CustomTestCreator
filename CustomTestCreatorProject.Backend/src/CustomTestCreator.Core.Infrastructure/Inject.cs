using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.Core.Application.Repositories;
using CustomTestCreator.Core.Infrastructure.DbContexts;
using CustomTestCreator.SharedKernel;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Core.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddFromCoreInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IReadDbContext, ReadDbContext>();
        
        return services;
    }
}