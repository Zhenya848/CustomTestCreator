namespace CustomTestCreator.Accounts.Contracts;

public interface IAccountsContract
{
    Task<IReadOnlyList<string>> GetUserPermissionCodes(Guid userId);
}