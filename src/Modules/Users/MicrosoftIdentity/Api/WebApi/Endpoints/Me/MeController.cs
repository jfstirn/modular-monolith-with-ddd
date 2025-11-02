using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.AuthenticatorRegistration.GetAuthenticatorKey;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.AuthenticatorRegistration.RegisterAuthenticator;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.ChangeEmailAddress;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.ChangePassword;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.ConfirmEmailAddress;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.GetUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.RequestChangeEmailAddressToken;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.RequestConfirmEmailAddressToken;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.UpdateProfile;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Me;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Me;

[Route("api/users/me")]
public class MeController : ApplicationController
{
    private readonly IUserAccessModule _userAccessModule;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public MeController(IUserAccessModule userAccessModule, IExecutionContextAccessor executionContextAccessor)
    {
        _userAccessModule = userAccessModule;
        _executionContextAccessor = executionContextAccessor;
    }

    /// <summary>
    /// Retrieves the account information for the currently authenticated user.
    /// </summary>
    /// <remarks>This endpoint does not require explicit permissions; any authenticated user may access their own account details.</remarks>
    /// <returns>An <see cref="IResult"/> containing the user's account details if the request is successful; otherwise, an error
    /// result indicating the reason for failure, such as invalid request or unauthorized access.</returns>
    [HttpGet]
    [NoPermissionRequired]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    public async Task<IResult> GetUserAccount()
    {
        var result = await _userAccessModule.ExecuteQueryAsync(new GetUserAccountQuery(_executionContextAccessor.UserId));
        if (result.IsSuccess && result.Value is not null)
        {
            return result.ToApiResult(new UserAccountResponse()
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                FirstName = result.Value.FirstName,
                LastName = result.Value.LastName,
                UserName = result.Value.UserName ?? string.Empty,
                EmailAddress = result.Value.EmailAddress
            });
        }

        return ToApiResult(result);
    }

    /// <summary>
    /// Updates the current user's profile information with the specified details.
    /// </summary>
    /// <param name="request">An object containing the new profile information to apply.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the update operation. Returns a success result if the profile
    /// was updated; otherwise, returns a result describing the failure.</returns>
    [HttpPut("update-profile")]
    [NoPermissionRequired]
    public async Task<IResult> UpdateProfile(UpdateProfileRequest request)
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new UpdateProfileCommand(_executionContextAccessor.UserId, request.Login, request.Name, request.FirstName, request.LastName));
        if (!result.IsSuccess)
        {
            return ToApiResult(result);
        }

        return Ok();
    }

    /// <summary>
    /// Attempts to change the current user's password using the provided credentials.
    /// </summary>
    /// <remarks>This operation does not require special permissions; any authenticated user may change their
    /// own password. The result may indicate failure if the current password is incorrect or if the new password does
    /// not meet policy requirements.</remarks>
    /// <param name="request">An object containing the current password and the new password to be set. The current password must be valid;
    /// the new password must meet any required password policies.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the password change operation. Returns a success result if
    /// the password was changed; otherwise, returns an error result describing the failure.</returns>
    [HttpPut("change-password")]
    [NoPermissionRequired]
    public async Task<IResult> ChangePassword(ChangePasswordRequest request)
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new ChangePasswordCommand(_executionContextAccessor.UserId, request.CurrentPassword, request.NewPassword));
        if (!result.IsSuccess)
        {
            return ToApiResult(result);
        }

        return Ok();
    }

    /// <summary>
    /// Initiates a request to generate a token for changing the currently authenticated user's email address.
    /// </summary>
    /// <remarks>This endpoint does not require specific permissions; any authenticated user may request a token to change their own email address.</remarks>
    /// <param name="request">An object containing the new email address to associate with the user's account. The new email address must be
    /// valid and not already in use.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the request. Returns a success result if the token is
    /// generated; otherwise, returns an error result describing the failure.</returns>
    [HttpGet("request-change-email-address-token")]
    [NoPermissionRequired]
    public async Task<IResult> RequestChangeEmailAddressToken(RequestChangeEmailAddressTokenRequest request)
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new RequestChangeEmailAddressTokenCommand(_executionContextAccessor.UserId, request.NewEmailAddress));
        if (!result.IsSuccess)
        {
            return ToApiResult(result);
        }

        return Ok();
    }

    /// <summary>
    /// Initiates a request to change the authenticated user's email address using the provided verification token and
    /// new email address.
    /// </summary>
    /// <remarks>This operation does not require special permissions; any authenticated user may change their own email address.</remarks>
    /// <param name="request">An object containing the new email address and the verification token required to authorize the change.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the email address change operation. Returns a success result
    /// if the change is completed; otherwise, returns an error result describing the failure.</returns>
    [HttpPut("change-email-address")]
    [NoPermissionRequired]
    public async Task<IResult> ChangeEmailAddress(ChangeEmailAddressRequest request)
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new ChangeEmailAddressCommand(_executionContextAccessor.UserId, request.NewEmailAddress, request.Token));
        if (!result.IsSuccess)
        {
            return ToApiResult(result);
        }

        return Ok();
    }

    /// <summary>
    /// Initiates a request to generate a confirmation token for the currently authenticated user's email address.
    /// </summary>
    /// <remarks>This endpoint does not require specific permissions. The confirmation token
    /// is typically sent to the user's registered email address and can be used to verify ownership of the
    /// email.</remarks>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the request. Returns a success result if the token was
    /// generated and sent; otherwise, returns an error result describing the failure.</returns>
    [HttpGet("request-confirm-email-address-token")]
    [NoPermissionRequired]
    public async Task<IResult> RequestConfirmEmailAddressToken()
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new RequestConfirmEmailAddressTokenCommand(_executionContextAccessor.UserId));
        if (!result.IsSuccess)
        {
            return ToApiResult(result);
        }

        return Ok();
    }

    /// <summary>
    /// Confirms a user's email address using the provided confirmation token.
    /// </summary>
    /// <remarks>This operation does not require special permissions; any authenticated user may confirm their own email address.</remarks>
    /// <param name="request">An object containing the email confirmation token required to verify the user's email address. Cannot be null.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the email confirmation operation. Returns a success result if
    /// the email address is confirmed; otherwise, returns an error result describing the failure.</returns>
    [HttpPut("confirm-email-address")]
    [NoPermissionRequired]
    public async Task<IResult> ConfirmEmailAddress(ConfirmEmailAddressRequest request)
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new ConfirmEmailAddressCommand(_executionContextAccessor.UserId, request.Token));
        if (!result.IsSuccess)
        {
            return ToApiResult(result);
        }

        return Ok();
    }

    /// <summary>
    /// Retrieves the authenticator key for the current user to enable two-factor authentication setup.
    /// </summary>
    /// <remarks>This endpoint does not require special permissions; any authenticated user may retrieve their own authenticator key.
    /// The authenticator key can be used to configure an authenticator app for two-factor authentication. If the operation fails, the result
    /// will include error details.</remarks>
    /// <returns>An <see cref="IResult"/> containing the authenticator key if retrieval is successful; otherwise, an error result
    /// describing the failure.</returns>
    [HttpGet("authenticator-key")]
    [NoPermissionRequired]
    public async Task<IResult> GetAuthenticatorKey()
    {
        var result = await _userAccessModule.ExecuteQueryAsync(new GetAuthenticatorKeyQuery(_executionContextAccessor.UserId));
        if (!result.IsSuccess)
        {
            return ToApiResult(result);
        }

        return result.ToApiResult(result.Value!);
    }

    /// <summary>
    /// Registers a new authenticator for the current user using the provided registration code by the authenticator app.
    /// </summary>
    /// <remarks>This operation does not require special permissions; any authenticated user may register their own authenticator.</remarks>
    /// <param name="request">The request containing the registration code required to register the authenticator. Cannot be null.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the registration operation. Returns a success result if the
    /// authenticator is registered; otherwise, returns an error result describing the failure.</returns>
    [HttpPost("register-authenticator")]
    [NoPermissionRequired]
    public async Task<IResult> RegisterAuthenticator(RegisterAuthenticatorRequest request)
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new RegisterAuthenticatorCommand(_executionContextAccessor.UserId, request.Code));
        if (!result.IsSuccess)
        {
            return ToApiResult(result);
        }

        return Ok();
    }
}