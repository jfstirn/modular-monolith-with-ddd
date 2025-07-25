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
    /// User login.
    /// </summary>
    /// <param name="request">Authentication attributes.</param>
    /// <returns>ApiResult.</returns>
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

            return response.ToResult(result).ToApiResult();
        }

        return Error(Errors.General.InvalidRequest());
    }

    /// <summary>
    /// User login.
    /// </summary>
    /// <param name="token">User generated token.</param>
    /// <returns>ApiResult.</returns>
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
    /// Send forgot password link.
    /// </summary>
    /// <param name="emailAddress">Email address of the user.</param>
    /// <returns>ApiResult.</returns>
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
    /// Reset password.
    /// </summary>
    /// <param name="resetPassword">Reset password attributes.</param>
    /// <returns>ApiResult.</returns>
    [HttpPost("reset-password")]
    [NoPermissionRequired]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    public async Task<IResult> ResetPassword(ResetPasswordRequest resetPassword)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new ResetPasswordCommand(resetPassword.Token, resetPassword.EmailAddress, resetPassword.Password));
        return response.ToApiResult();
    }

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
                    return response.ToApiResult(authenticationResult);
                }

                return Error(Errors.Authentication.InvalidToken());
            }
        }

        return Error(Errors.General.InvalidRequest());
    }

    [HttpPost("refresh-token")]
    [NoPermissionRequired]
    public async Task<IResult> RefreshToken(TokenRequest tokenRequest)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new RefreshTokenCommand(tokenRequest.AccessToken, tokenRequest.RefreshToken));
        if (!response.IsSuccess)
        {
            return FromResponse(response);
        }

        var tokenResult = new TokenResponse()
        {
            AccessToken = response.Value!.AccessToken,
            RefreshToken = response.Value!.RefreshToken
        };
        return response.ToApiResult(tokenResult);
    }
}