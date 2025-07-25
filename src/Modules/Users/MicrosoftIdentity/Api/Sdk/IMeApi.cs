using CompanyName.MyMeetings.Modules.UsersMI.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Me;
using Refit;

namespace CompanyName.MyMeetings.Modules.UsersMI.Sdk;

public interface IMeApi
{
    [Get(ApiEndpoints.Me.GetUserAccount)]
    Task<Result<UserAccountResponse>> GetUserAccountAsync(CancellationToken cancellationToken = default);

    [Put(ApiEndpoints.Me.UpdateProfile)]
    Task<Result> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default);

    [Put(ApiEndpoints.Me.ChangePassword)]
    Task<Result> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default);

    [Get(ApiEndpoints.Me.GetAuthenticatorKey)]
    Task<Result<string>> GetAuthenticatorKeyAsync(CancellationToken cancellationToken = default);

    [Put(ApiEndpoints.Me.ChangeEmailAddress)]
    Task<Result> ChangeEmailAddressAsync(ChangeEmailAddressRequest request, CancellationToken cancellationToken = default);

    [Put(ApiEndpoints.Me.ConfirmEmailAddress)]
    Task<Result> ConfirmEmailAddressAsync(ConfirmEmailAddressRequest request, CancellationToken cancellationToken = default);

    [Post(ApiEndpoints.Me.RegisterAuthenticator)]
    Task<Result> RegisterAuthenticatorAsync(RegisterAuthenticatorRequest request, CancellationToken cancellationToken = default);

    [Get(ApiEndpoints.Me.RequestChangeEmailAddressToken)]
    Task<Result> RequestChangeEmailAddressTokenAsync(RequestChangeEmailAddressTokenRequest request, CancellationToken cancellationToken = default);

    [Get(ApiEndpoints.Me.RequestConfirmEmailAddressToken)]
    Task<Result> RequestConfirmEmailAddressTokenAsync(CancellationToken cancellationToken = default);
}