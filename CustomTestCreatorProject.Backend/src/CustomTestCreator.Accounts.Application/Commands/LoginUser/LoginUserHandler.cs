using CSharpFunctionalExtensions;
using CustomTestCreator.Accounts.Domain;
using CustomTestCreator.Accounts.Domain.User;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CustomTestCreator.Accounts.Application.Commands.LoginUser;

public class LoginUserHandler : ICommandHandler<LoginUserCommand, Result<UserDataDto, ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<User> _logger;
    private readonly ITokenProvider _tokenProvider;

    public LoginUserHandler(
        UserManager<User> userManager,
        ILogger<User> logger,
        ITokenProvider tokenProvider)
    {
        _userManager = userManager;
        _logger = logger;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<Result<UserDataDto, ErrorList>> Handle(
        LoginUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager
            .FindByEmailAsync(command.Email);
        
        if (user == null)
            return (ErrorList)Errors.User.NotFound(command.Email);
        
        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        
        if (passwordConfirmed == false)
            return (ErrorList)Errors.User.WrongCredentials();
        
        var accessToken = _tokenProvider.GenerateAccessToken(user);
        
        _logger.LogInformation("Login successfully");

        var response = new UserDataDto(accessToken.AccessToken, user.Id);
        
        return response;
    }
}