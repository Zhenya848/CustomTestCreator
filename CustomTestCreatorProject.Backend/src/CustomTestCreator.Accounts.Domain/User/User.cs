using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.AspNetCore.Identity;

namespace CustomTestCreator.Accounts.Domain.User;

public class User : IdentityUser<Guid>
{
    private List<Role> _roles;
    public IReadOnlyList<Role> Roles => _roles;

    private User()
    {
        
    }

    public static User Create(Role role, string username, string email) =>
        new User { UserName = username, Email = email, _roles = [role] };
}