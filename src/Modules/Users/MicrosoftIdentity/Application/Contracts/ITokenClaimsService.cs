using System.Security.Claims;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using CSharpFunctionalExtensions;
using Microsoft.IdentityModel.Tokens;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;

public interface ITokenClaimsService
{
    TokenValidationParameters GetTokenValidationParameters();

    Tokens GenerateTokens(ApplicationUser user);

    Task<Result<Tokens, Error>> GenerateNewTokensAsync(string accessToken, string refreshToken, CancellationToken cancellationToken);

    List<Claim> GetUserClaims(ApplicationUser user);
}

public record Tokens(string AccessToken, string RefreshToken);