using System.Security.Claims;
using CSharpFunctionalExtensions;
using CustomTestCreator.Accounts.Application.Models;
using CustomTestCreator.Accounts.Domain.User;
using CustomTestCreator.SharedKernel;

namespace CustomTestCreator.Accounts.Application;

public interface ITokenProvider
{
    JwtTokenResult GenerateAccessToken(User user);
    Task<Result<IReadOnlyList<Claim>, ErrorList>> GetUserClaims(string jwtToken);
}