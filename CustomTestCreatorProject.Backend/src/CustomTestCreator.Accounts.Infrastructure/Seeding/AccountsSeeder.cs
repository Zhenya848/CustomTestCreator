using Microsoft.Extensions.DependencyInjection;

namespace CustomTestCreator.Accounts.Infrastructure.Seeding;

public class AccountsSeeder(IServiceScopeFactory scopeFactory)
{
    public async Task SeedAsync()
    {
        using var scope = scopeFactory.CreateScope();
        
        var service  = scope.ServiceProvider.GetRequiredService<AccountsSeederService>();

        await service.SeedAsync();
    }
}