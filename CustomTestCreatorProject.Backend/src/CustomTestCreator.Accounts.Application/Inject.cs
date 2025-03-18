using CustomTestCreator.SharedKernel.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Accounts.Application;

public static class Inject
{
    public static IServiceCollection AddFromAccountsApplication(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes.AssignableToAny(
                typeof(ICommandHandler<,>),
                typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithLifetime(ServiceLifetime.Scoped));
    }
}