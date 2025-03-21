using System.Text.Json;
using CustomTestCreator.Accounts.Domain.Constants;
using CustomTestCreator.Accounts.Domain.User;
using CustomTestCreator.Accounts.Infrastructure.DbContexts;
using CustomTestCreator.Accounts.Infrastructure.Options;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CustomTestCreator.Accounts.Infrastructure.Seeding;

public class AccountsSeederService(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    AccountDbContext accountsDbContext,
    ILogger<AccountsSeederService> logger)
{
    public async Task SeedAsync()
    {
        var json = await File.ReadAllTextAsync("etc/accounts.json");

        var seedData = JsonSerializer.Deserialize<RolePermissionOptions>(json)
                       ?? throw new ApplicationException("Role Permission Config is missing");

        await SeedPermissionsAsync(seedData);
        
        logger.LogInformation("Seeded permission successfully");
        
        await SeedRolesAsync(seedData);
        
        logger.LogInformation("Seeded roles successfully");

        await accountsDbContext.SaveChangesAsync();
        
        logger.LogInformation("Saved permissions and roles successfully");
        
        await SeedRolePermissionsAsync(seedData);
        
        logger.LogInformation("Seeded role_permissions successfully");
        
        var adminRole = await roleManager.FindByNameAsync(AccountRoles.ADMIN) 
            ?? throw new ApplicationException("Admin Role not found");
        
        var adminUser = await userManager.Users
            .FirstOrDefaultAsync(u => u.Roles.Any(r => r.Name == adminRole.Name));

        if (adminUser == null)
        {
            adminUser = User.Create(adminRole, AdminOptions.ADMIN_USERNAME, AdminOptions.ADMIN_EMAIL);
        
            await userManager.CreateAsync(adminUser, AdminOptions.ADMIN_PASSWORD);
        }
        
        logger.LogInformation("Created user admin successfully");
        
        await accountsDbContext.SaveChangesAsync();
        
        logger.LogInformation("Saved all data successfully");
    }
    
    private async Task SeedPermissionsAsync(RolePermissionOptions seedData)
    {
        var keys = seedData.Permissions.Keys.ToList();
        var permissionsToAdd = new List<string>();
        
        foreach (var key in keys)
        {
            var permissions = seedData.Permissions[key];

            permissionsToAdd.AddRange(permissions);
        }
        
        foreach (var permissionCode in permissionsToAdd)
        {
            var isPermissionExist = await accountsDbContext.Permissions.AnyAsync(p => p.Code == permissionCode);

            if (isPermissionExist == false)
                await accountsDbContext.Permissions.AddAsync(new Permission() 
                    { Code = permissionCode, Description = permissionCode} );
        }
    }
    
    private async Task SeedRolesAsync(RolePermissionOptions seedData)
    {
        foreach (var role in seedData.Roles.Keys)
        {
            var isRoleExist = await roleManager.FindByNameAsync(role);

            if (isRoleExist is null)
                await roleManager.CreateAsync(new Role() { Name = role });
        }
    }

    private async Task SeedRolePermissionsAsync(RolePermissionOptions seedData)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var roleExist = await roleManager.FindByNameAsync(roleName);
            
            if (roleExist is null)
                throw new ApplicationException($"Role {roleName} does not exist");

            foreach (var permissionCode in seedData.Roles[roleName])
            {
                var permissionExist = await accountsDbContext.Permissions
                    .FirstOrDefaultAsync(p => p.Code == permissionCode);
                
                if (permissionExist is null)
                    throw new ApplicationException($"Permission {permissionCode} does not exist");

                var rolePermission = await accountsDbContext.RolePermissions.AnyAsync(rp =>
                    rp.RoleId == roleExist.Id && rp.PermissionId == permissionExist.Id);
                
                if (rolePermission == false)
                    await accountsDbContext.RolePermissions.AddAsync(new RolePermission() 
                        { RoleId = roleExist.Id, PermissionId = permissionExist.Id });
            }
        }
    }
}