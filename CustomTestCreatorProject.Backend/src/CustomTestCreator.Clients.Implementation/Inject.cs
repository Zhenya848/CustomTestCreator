using CustomTestCreator.Clients.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Clients.Implementation;

public static class Inject
{
    public static IServiceCollection AddFromClientsContract(this IServiceCollection services)
    {
        return services.AddScoped<IClientsContract, ClientsContract>();
    }
}