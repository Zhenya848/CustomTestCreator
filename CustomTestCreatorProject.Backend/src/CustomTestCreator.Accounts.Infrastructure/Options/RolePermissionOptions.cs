namespace CustomTestCreator.Accounts.Infrastructure.Options;

public class RolePermissionOptions
{
    public Dictionary<string, string[]> Permissions { get; set; } = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> Roles { get; set; } = new Dictionary<string, string[]>();
}