using Microsoft.AspNetCore.Identity;

namespace CustomTestCreator.Accounts.Domain.User;

public class User : IdentityUser<Guid>
{
    private List<Role> _roles;
    public IReadOnlyList<Role> Roles => _roles;

    private User()
    {
        
    }

    public static User Create(Role role, string userName) =>
        new User { UserName = userName, _roles = [role] };
}