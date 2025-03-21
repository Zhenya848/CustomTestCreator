using System.Net.Mail;
using CSharpFunctionalExtensions;
using CustomTestCreator.Accounts.Application.Commands.LoginUser;
using CustomTestCreator.Accounts.Domain;
using CustomTestCreator.Accounts.Domain.User;
using CustomTestCreator.Clients.Contracts;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CustomTestCreator.Accounts.Application.Commands.CreateUser;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, Result<UserDataDto, ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<CreateUserHandler> _logger;
    private readonly IClientsContract _clientsContract;
    private readonly LoginUserHandler _loginUserHandler;

    public CreateUserHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<CreateUserHandler> logger,
        IClientsContract clientsContract,
        LoginUserHandler loginUserHandler)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _clientsContract = clientsContract;
        _loginUserHandler = loginUserHandler;
    }
    
    public async Task<Result<UserDataDto, ErrorList>> Handle(
        CreateUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Username))
            return (ErrorList)Errors.General.ValueIsRequired("Email");
        
        if (EmailValidator.IsVaild(command.Email) == false)
            return (ErrorList)Errors.General.ValueIsInvalid("Email");
        
        var userExist = await _userManager.FindByEmailAsync(command.Email);

        if (userExist != null)
            return (ErrorList)Errors.User.AlreadyExist();
        
        var role = await _roleManager.FindByNameAsync(AccountRoles.PARTICIPANT)
                   ?? throw new ApplicationException($"Role {AccountRoles.PARTICIPANT} does not exist");
        
        var user = User.Create(role, command.Username, command.Email);
        
        var result = await _userManager.CreateAsync(user, command.Password);

        if (result.Succeeded == false)
            return (ErrorList)result.Errors
                .Select(e => Error.Failure(e.Code, e.Description)).ToList();
        
        _logger.LogInformation("User created: {userName} a new account with password.", user.UserName);
        
        var loginCommand = new LoginUserCommand(command.Email, command.Password);
        var loginResult = await _loginUserHandler.Handle(loginCommand, cancellationToken);
        
        if (loginResult.IsFailure)
            return loginResult.Error;
        
        await _clientsContract.CreateClient(ClientId.Create(user.Id), user.UserName!, true, true, cancellationToken);

        var response = loginResult.Value;

        return response;
    }
}