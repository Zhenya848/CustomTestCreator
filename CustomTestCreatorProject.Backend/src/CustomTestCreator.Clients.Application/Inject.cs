using System.ComponentModel.Design;
using CustomTestCreator.SharedKernel.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Clients.Application;

public static class Inject
{
    public static IServiceCollection AddFromClientsApplication(this IServiceCollection services)
    {
        var assembly = typeof(Inject).Assembly;
        
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableToAny(
                typeof(ICommandHandler<,>),
                typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithLifetime(ServiceLifetime.Scoped));
        
        return services.AddValidatorsFromAssembly(assembly);
    }
}