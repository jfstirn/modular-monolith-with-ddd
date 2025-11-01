using System.Security.Claims;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.Login;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.Login.External;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.RefreshToken;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.RequestResetPasswordToken;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.ResetPassword;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Authentication;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Authentication;

[AllowAnonymous]
[Route("api/authentication")]
public class AuthenticationController : ApplicationController
{
    private readonly IUserAccessModule _userAccessModule;

    public AuthenticationController(IUserAccessModule userAccessModule)
    {
        _userAccessModule = userAccessModule;
    }

    /// <summary>
    /// Authenticates a user using the provided credentials and initiates a login session. Supports two-factor
    /// authentication if required by the user's account.
    /// </summary>
    /// <remarks>If the user's account requires two-factor authentication, the response will indicate this and
    /// the user will be prompted to complete the additional verification step. Otherwise, a successful login will
    /// establish an authenticated session and return access and refresh tokens. This endpoint does not require prior
    /// permissions.</remarks>
    /// <param name="request">An <see cref="AuthenticationRequest"/> containing the user's login credentials. The <c>UserName</c> and
    /// <c>Password</c> properties must be provided and valid.</param>
    /// <returns>An <see cref="IResult"/> representing the outcome of the login attempt. Returns a successful result with
    /// authentication details if login succeeds, or an error result if the request is invalid or authentication fails.</returns>
    [HttpPost("login")]
    [NoPermissionRequired]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    public async Task<IResult> Login(AuthenticationRequest request)
    {
        AuthenticationResponse? result = null;

        var response = await _userAccessModule.ExecuteCommandAsync(new AccountLoginCommand(request.UserName, request.Password));
        if (response is not null)
        {
            if (response.IsSuccess)
            {
                if (response.RequiresTwoFactor)
                {
                    await HttpContext.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, response.ClaimsPrincipal!);

                    result = new AuthenticationResponse()
                    {
                        RequiresTwoFactor = true
                    };
                }
                else if (response.IsAuthenticated)
                {
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, response.ClaimsPrincipal!);

                    result = new AuthenticationResponse()
                    {
                        UserName = response.User!.UserName,
                        AccessToken = response.AccessToken,
                        RefreshToken = response.RefreshToken
                    };
                }

                return Ok(result);
            }

            return response.ToApiResult();
        }

        return Error(Errors.General.InvalidRequest());
    }

    /// <summary>
    /// Attempts to authenticate a user using a two-factor authentication token as part of the login process.
    /// </summary>
    /// <remarks>This endpoint should be called after initiating a two-factor authentication flow. If the
    /// login request has expired or the token is invalid, an error response is returned. Upon successful
    /// authentication, the user's session is established and relevant authentication tokens are issued.</remarks>
    /// <param name="token">The two-factor authentication token provided by the user. This value must be valid and correspond to the current
    /// two-factor authentication session.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the authentication attempt. Returns a successful result with
    /// authentication details if the token is valid; otherwise, returns an error result describing the failure.</returns>
    [HttpPost("two-factor-login")]
    [NoPermissionRequired]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    public async Task<IResult> TwoFactorLogin(string token)
    {
        var result = await HttpContext.AuthenticateAsync(IdentityConstants.TwoFactorUserIdScheme);
        if (result is not null)
        {
            if (!result.Succeeded)
            {
                return Error(Errors.Authentication.LoginRequestExpired());
            }

            var response = await _userAccessModule.ExecuteCommandAsync(new AccountTwoFactorLoginCommand(
                    Guid.Parse(result.Principal.FindFirstValue("sub")!),
                    result.Principal.FindFirstValue("amr")!,
                    token));

            if (response is not null)
            {
                if (response.IsAuthenticated)
                {
                    // Clean up the cookie
                    await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, response.ClaimsPrincipal!);

                    AuthenticationResponse authenticationResult = new AuthenticationResponse()
                    {
                        UserName = response.User!.UserName,
                        AccessToken = response.AccessToken,
                        RefreshToken = response.RefreshToken
                    };
                    return Ok(authenticationResult);
                }

                return Error(Errors.Authentication.InvalidToken());
            }
        }

        return Error(Errors.General.InvalidRequest());
    }

    /// <summary>
    /// Attempts to refresh the access token using the provided refresh token and returns the result of the operation.
    /// </summary>
    /// <remarks>This endpoint does not require authentication. The caller must supply valid tokens;
    /// otherwise, the operation will fail and an error result will be returned.</remarks>
    /// <param name="tokenRequest">An object containing the current access token and refresh token to be used for refreshing authentication
    /// credentials.</param>
    /// <returns>An <see cref="IResult"/> representing the outcome of the token refresh operation. If successful, contains a new
    /// access token and refresh token; otherwise, includes error details.</returns>
    [HttpPost("refresh-token")]
    [NoPermissionRequired]
    public async Task<IResult> RefreshToken(TokenRequest tokenRequest)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new RefreshTokenCommand(tokenRequest.AccessToken, tokenRequest.RefreshToken));
        if (!response.IsSuccess)
        {
            return ToApiResult(response);
        }

        var tokenResult = new TokenResponse()
        {
            AccessToken = response.Value!.AccessToken,
            RefreshToken = response.Value!.RefreshToken
        };
        return response.ToApiResult(tokenResult);
    }

    /// <summary>
    /// Initiates a password reset process by requesting a reset token for the specified email address.
    /// </summary>
    /// <param name="emailAddress">The email address associated with the user account for which the password reset token is requested.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the request. Returns a success result if the token was
    /// requested successfully; otherwise, returns a failure result with error details.</returns>
    [HttpPost("request-reset-password-token")]
    [NoPermissionRequired]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    public async Task<IResult> RequestResetPasswordToken(string emailAddress)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new RequestResetPasswordTokenCommand(emailAddress));
        return response.ToApiResult();
    }

    /// <summary>
    /// Resets the user's password using the provided reset token, email address, and new password.
    /// </summary>
    /// <param name="resetPassword">An object containing the reset token, the user's email address, and the new password to set. All fields must be
    /// valid and non-empty.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the password reset operation. Returns a success result if the
    /// password was reset; otherwise, returns an error result describing the failure.</returns>
    [HttpPost("reset-password")]
    [NoPermissionRequired]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    public async Task<IResult> ResetPassword(ResetPasswordRequest resetPassword)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new ResetPasswordCommand(resetPassword.Token, resetPassword.EmailAddress, resetPassword.Password));
        return response.ToApiResult();
    }

    /// <summary>
    /// Initiates an authentication challenge using the specified external login provider.
    /// </summary>
    /// <remarks>This endpoint does not require prior authentication and is typically used to start an OAuth
    /// or similar external login flow. The user will be redirected to the provider's login page, and upon successful
    /// authentication, will be returned to the application's callback endpoint.</remarks>
    /// <param name="provider">The name of the external authentication provider to use for login. This value must correspond to a configured
    /// authentication scheme (for example, "Google" or "Facebook").</param>
    /// <returns>An <see cref="IResult"/> that triggers the authentication challenge for the specified provider. The response
    /// will redirect the user to the external provider's login page.</returns>
    [HttpGet("external-login")]
    [NoPermissionRequired]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IResult ExternalLogin(string provider)
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(ExternalLoginCallback)),
            Items = { { "scheme", provider } }
        };
        return Results.Challenge(properties, [provider]);
    }

    /// <summary>
    /// Handles the callback from an external authentication provider and completes the sign-in process for the
    /// authenticated user.
    /// </summary>
    /// <remarks>This endpoint is typically invoked by external identity providers after a user has
    /// authenticated. It validates the external authentication response, extracts required user information, and signs
    /// the user into the application if authentication is successful. If required information is missing or
    /// authentication fails, an appropriate error response is returned. No authentication or permission is required to
    /// access this endpoint.</remarks>
    /// <returns>An <see cref="IResult"/> representing the outcome of the external login process. Returns a successful result if
    /// authentication is completed; otherwise, returns an error result indicating the reason for failure.</returns>
    [HttpGet("external-login-callback")]
    [NoPermissionRequired]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    public async Task<IResult> ExternalLoginCallback()
    {
        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
        if (result != null)
        {
            // We really need the external user identifier
            var externalUserId = result.Principal?.FindFirstValue("sub")
                ?? result.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (externalUserId is null)
            {
                return Error(Errors.General.InvalidRequest("Cannot find external user id"));
            }

            // Get the provider from the authentication properties which is available from the scheme item
            var provider = result.Properties?.Items["scheme"];
            if (provider is null)
            {
                return Error(Errors.General.InvalidRequest("Missing external provider"));
            }

            var emailAddress = result.Principal?.FindFirstValue("email")
                ?? result.Principal?.FindFirstValue(ClaimTypes.Email);
            if (emailAddress is null)
            {
                return Error(Errors.General.InvalidRequest("Email address must be provided"));
            }

            // Once we have all this we can go ahead an call the external login command
            var response = await _userAccessModule.ExecuteCommandAsync(new ExternalAccountLoginCommand(provider, externalUserId, emailAddress, false));

            if (response != null)
            {
                if (response.IsAuthenticated)
                {
                    // Clean up the cookie
                    await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, response.ClaimsPrincipal!);

                    var authenticationResult = new AuthenticationResponse()
                    {
                        UserName = response.User!.UserName,
                        AccessToken = response.AccessToken,
                        RefreshToken = response.RefreshToken
                    };
                    return Ok(authenticationResult);
                }

                return Error(Errors.Authentication.InvalidToken());
            }
        }

        return Error(Errors.General.InvalidRequest());
    }
}