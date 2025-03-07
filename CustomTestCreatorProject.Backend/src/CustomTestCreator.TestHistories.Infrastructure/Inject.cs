using CustomTestCreator.TestHistories.Infrastructure.DbContexts;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.TestHistories.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddFromTestHistoriesInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<TestHistoryDbContext>();
        
        return services;
    }
}