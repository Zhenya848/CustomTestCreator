using CustomTestCreator.Accounts.Contracts;
using CustomTestCreator.Accounts.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CustomTestCreator.Accounts.Implementation;

public class AccountsContract : IAccountsContract
{
    private readonly AccountDbContext _accountDbContext;

    public AccountsContract(AccountDbContext accountDbContext)
    {
        _accountDbContext = accountDbContext;
    }
    
    public async Task<IReadOnlyList<string>> GetUserPermissionCodes(Guid userId)
    {
        var permissions = await _accountDbContext.Users
            .Include(r => r.Roles)
            .Where(u => u.Id == userId)
            .SelectMany(r => r.Roles)
            .SelectMany(rp => rp.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .ToListAsync();
        
        return permissions;
    }
}