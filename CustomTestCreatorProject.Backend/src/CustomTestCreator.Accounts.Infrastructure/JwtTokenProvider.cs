using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using CustomTestCreator.Accounts.Application;
using CustomTestCreator.Accounts.Application.Models;
using CustomTestCreator.Accounts.Domain.User;
using CustomTestCreator.Accounts.Infrastructure.DbContexts;
using CustomTestCreator.Accounts.Presentation;
using CustomTestCreator.Accounts.Presentation.Options;
using CustomTestCreator.SharedKernel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CustomTestCreator.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly AccountDbContext _accountDbContext;

    public JwtTokenProvider(
        IOptions<JwtOptions> jwtOptions,
        AccountDbContext accountsDbContext)
    {
        _jwtOptions = jwtOptions.Value;
        _accountDbContext = accountsDbContext;
    }
    
    public JwtTokenResult GenerateAccessToken(User user)
    {
        var jti = Guid.NewGuid();
        
        var claims = new[]
        {
            new Claim(CustomClaims.Sub, user.Id.ToString()),
            new Claim(CustomClaims.Jti, jti.ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiredMinutesTime),
            claims: claims,
            signingCredentials: creds
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        return new JwtTokenResult(tokenHandler.WriteToken(token), jti);
    }
    
    public async Task<Result<IReadOnlyList<Claim>, ErrorList>> GetUserClaims(string jwtToken)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        
        var validationParameters = TokenValidationParametersFactory.CreateWithoutLifeTime(_jwtOptions);

        var validationResult = await jwtHandler.ValidateTokenAsync(jwtToken, validationParameters);

        if (validationResult.IsValid == false)
            return (ErrorList)Errors.Token.InvalidToken();

        return validationResult.ClaimsIdentity.Claims.ToList();
    }
}