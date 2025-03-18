using CSharpFunctionalExtensions;
using CustomTestCreator.Accounts.Application.Repositories;
using CustomTestCreator.Accounts.Domain.User;
using CustomTestCreator.Accounts.Infrastructure.DbContexts;
using CustomTestCreator.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace CustomTestCreator.Accounts.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AccountDbContext _accountsDbContext;

    public AccountRepository(AccountDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }

    public async Task<Result<User, Error>> GetInfoAboutUser(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var user = await _accountsDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        
        if (user == null)
            return Errors.User.NotFound();
        
        return user;
    }

    public async Task<IEnumerable<User>> GetUsers(
        IEnumerable<string> users, 
        IEnumerable<string> roles, 
        CancellationToken cancellationToken = default)
    {
        var usersExist = await _accountsDbContext.Users
            .Include(r => r.Roles)
            .Where(u => users.Contains(u.UserName) || u.Roles.Any(role => roles.Contains(role.Name)))
            .ToListAsync(cancellationToken);

        return usersExist;
    }
}