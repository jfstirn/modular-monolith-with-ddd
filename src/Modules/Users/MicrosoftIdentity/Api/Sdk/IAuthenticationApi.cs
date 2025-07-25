using CompanyName.MyMeetings.Modules.UsersMI.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Authentication;
using Refit;

namespace CompanyName.MyMeetings.Modules.UsersMI.Sdk;

public interface IAuthenticationApi
{
    [Post(ApiEndpoints.Authentication.Login)]
    Task<Result<AuthenticationResponse>> LoginAsync(AuthenticationRequest request, CancellationToken cancellationToken = default);

    [Post(ApiEndpoints.Authentication.TwoFactorLogin)]
    Task<Result<AuthenticationResponse>> TwoFactorLoginAsync(string token, CancellationToken cancellationToken = default);

    [Post(ApiEndpoints.Authentication.RequestForgotPasswordLink)]
    Task<Result<string>> RequestForgotPasswordLinkAsync(string emailAddress, CancellationToken cancellationToken = default);

    [Post(ApiEndpoints.Authentication.ResetPassword)]
    Task<Result> ResetPasswordAsync(ResetPasswordRequest resetPassword, CancellationToken cancellationToken = default);

    [Post(ApiEndpoints.Authentication.RefreshToken)]
    Task<Result<TokenResponse>> RefreshTokenAsync(TokenRequest request, CancellationToken cancellationToken = default);
}