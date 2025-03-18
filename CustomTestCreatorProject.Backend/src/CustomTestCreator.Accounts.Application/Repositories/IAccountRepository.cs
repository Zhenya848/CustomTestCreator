using CSharpFunctionalExtensions;
using CustomTestCreator.Accounts.Domain.User;
using CustomTestCreator.SharedKernel;

namespace CustomTestCreator.Accounts.Application.Repositories;

public interface IAccountRepository
{
    public Task<Result<User, Error>> GetInfoAboutUser(
        Guid userId,
        CancellationToken cancellationToken = default);
    
    public Task<IEnumerable<User>> GetUsers(
        IEnumerable<string> users,
        IEnumerable<string> roles,
        CancellationToken cancellationToken = default);
}