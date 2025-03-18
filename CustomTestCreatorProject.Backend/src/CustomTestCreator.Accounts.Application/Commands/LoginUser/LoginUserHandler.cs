using CSharpFunctionalExtensions;
using CustomTestCreator.Accounts.Domain.User;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CustomTestCreator.Accounts.Application.Commands.LoginUser;

public class LoginUserHandler : ICommandHandler<LoginUserCommand, Result<string, ErrorList>>
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
    
    public async Task<Result<string, ErrorList>> Handle(
        LoginUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager
            .FindByNameAsync(command.Username);
        
        if (user == null)
            return (ErrorList)Errors.User.NotFound(command.Username);
        
        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        
        if (passwordConfirmed == false)
            return (ErrorList)Errors.User.WrongCredentials();
        
        var accessToken = _tokenProvider.GenerateAccessToken(user);
        
        _logger.LogInformation("Login successfully");
        
        return accessToken.AccessToken;
    }
}