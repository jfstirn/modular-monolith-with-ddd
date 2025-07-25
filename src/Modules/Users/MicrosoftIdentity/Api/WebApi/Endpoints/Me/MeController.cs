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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Me;

[Authorize]
[ApiController]
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

    [HttpGet("change-email-address")]
    [NoPermissionRequired]
    public async Task<IResult> ChangeEmailAddress(ChangeEmailAddressRequest request)
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new ChangeEmailAddressCommand(_executionContextAccessor.UserId, request.NewEmailAddress, request.Token));
        if (!result.IsSuccess)
        {
            return FromResponse(result);
        }

        return Ok();
    }

    [HttpPut("change-password")]
    [NoPermissionRequired]
    public async Task<IResult> ChangePassword(ChangePasswordRequest request)
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new ChangePasswordCommand(_executionContextAccessor.UserId, request.CurrentPassword, request.NewPassword));
        if (!result.IsSuccess)
        {
            return FromResponse(result);
        }

        return Ok();
    }

    [HttpPut("confirm-email-address")]
    [NoPermissionRequired]
    public async Task<IResult> ConfirmEmailAddress(ConfirmEmailAddressRequest request)
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new ConfirmEmailAddressCommand(_executionContextAccessor.UserId, request.Token));
        if (!result.IsSuccess)
        {
            return FromResponse(result);
        }

        return Ok();
    }

    [HttpGet("authenticator-key")]
    [NoPermissionRequired]
    public async Task<IResult> GetAuthenticatorKey()
    {
        var result = await _userAccessModule.ExecuteQueryAsync(new GetAuthenticatorKeyQuery(_executionContextAccessor.UserId));
        if (!result.IsSuccess)
        {
            return FromResponse(result);
        }

        return result.ToApiResult(result.Value!);
    }

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

        return FromResponse(result);
    }

    [HttpPost("register-authenticator")]
    [NoPermissionRequired]
    public async Task<IResult> RegisterAuthenticator(RegisterAuthenticatorRequest request)
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new RegisterAuthenticatorCommand(_executionContextAccessor.UserId, request.Code));
        if (!result.IsSuccess)
        {
            return FromResponse(result);
        }

        return Ok();
    }

    [HttpGet("request-change-email-address-token")]
    [NoPermissionRequired]
    public async Task<IResult> RequestChangeEmailAddressToken(RequestChangeEmailAddressTokenRequest request)
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new RequestChangeEmailAddressTokenCommand(_executionContextAccessor.UserId, request.NewEmailAddress));
        if (!result.IsSuccess)
        {
            return FromResponse(result);
        }

        return Ok();
    }

    [HttpGet("request-confirm-email-address-token")]
    [NoPermissionRequired]
    public async Task<IResult> RequestConfirmEmailAddressToken()
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new RequestConfirmEmailAddressTokenCommand(_executionContextAccessor.UserId));
        if (!result.IsSuccess)
        {
            return FromResponse(result);
        }

        return Ok();
    }

    [HttpPut("update-profile")]
    [NoPermissionRequired]
    public async Task<IResult> UpdateProfile(UpdateProfileRequest request)
    {
        var result = await _userAccessModule.ExecuteCommandAsync(new UpdateProfileCommand(_executionContextAccessor.UserId, request.Login, request.Name, request.FirstName, request.LastName));
        if (!result.IsSuccess)
        {
            return FromResponse(result);
        }

        return Ok();
    }
}