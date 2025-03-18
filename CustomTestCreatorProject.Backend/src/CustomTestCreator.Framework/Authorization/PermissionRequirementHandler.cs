using CustomTestCreator.Accounts.Contracts;
using CustomTestCreator.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Framework.Authorization;

public class PermissionRequirementHandler(IServiceScopeFactory scopeFactory) 
    : AuthorizationHandler<PermissionAttribute>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionAttribute requirement)
    {
        var userIdString = context.User.Claims
            .FirstOrDefault(c => c.Type == CustomClaims.Sub)?.Value;
        
        Guid userId;

        if (userIdString is null || Guid.TryParse(userIdString, out userId) == false)
        {
            context.Fail();
            return;
        }
        
        var scope = scopeFactory.CreateScope();
        var accountContract = scope.ServiceProvider.GetRequiredService<IAccountsContract>();
        
        var permissions = await accountContract.GetUserPermissionCodes(userId);
        
        if (permissions.Contains(requirement.Code))
            context.Succeed(requirement);
    }
}