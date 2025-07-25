using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.RefreshToken;

public class RefreshTokenCommand : CommandBase<Result<TokenDto>>
{
    public RefreshTokenCommand(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; }

    public string RefreshToken { get; }
}