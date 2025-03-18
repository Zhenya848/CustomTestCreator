using CustomTestCreator.Accounts.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Accounts.Implementation;

public static class Inject
{
    public static IServiceCollection AddFromAccountsContract(this IServiceCollection services)
    {
        return services.AddScoped<IAccountsContract, AccountsContract>();
    }
}