using CustomTestCreator.Framework.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Framework;

public static class Inject
{
    public static IServiceCollection AddFromFramework(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
            
        return services;
    }
}