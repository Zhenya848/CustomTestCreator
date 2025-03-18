using CSharpFunctionalExtensions;
using CustomTestCreator.Accounts.Domain.User;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CustomTestCreator.Accounts.Application.Commands.CreateUser;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, UnitResult<ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<CreateUserHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        CreateUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        var userExist = await _userManager.FindByNameAsync(command.Username);

        if (userExist != null)
            return (ErrorList)Errors.User.AlreadyExist();
        
        var role = await _roleManager.FindByNameAsync(AccountRoles.PARTICIPANT)
                   ?? throw new ApplicationException($"Role {AccountRoles.PARTICIPANT} does not exist");
        
        var user = User.Create(role, command.Username);
        
        var result = await _userManager.CreateAsync(user, command.Password);

        if (result.Succeeded == false)
            return (ErrorList)result.Errors
                .Select(e => Error.Failure(e.Code, e.Description)).ToList();
        
        _logger.LogInformation("User created: {userName} a new account with password.", user.UserName);

        return Result.Success<ErrorList>();
    }
}