using CustomTestCreator.Accounts.Infrastructur.DbContexts;
using CustomTestCreator.Core.Application.Abstractions;
using CustomTestCreator.SharedKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Accounts.Infrastructur;

public static class Inject
{
    public static IServiceCollection AddFromAccountsInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<AccountDbContext>();
        //services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Client);

        return services;
    }
}