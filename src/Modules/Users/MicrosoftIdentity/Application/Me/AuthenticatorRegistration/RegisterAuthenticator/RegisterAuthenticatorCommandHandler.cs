using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.AuthenticatorRegistration.RegisterAuthenticator;

internal class RegisterAuthenticatorCommandHandler : ICommandHandler<RegisterAuthenticatorCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public RegisterAuthenticatorCommandHandler(UserManager<ApplicationUser> userManager, IExecutionContextAccessor executionContextAccessor)
    {
        _userManager = userManager;
        _executionContextAccessor = executionContextAccessor;
    }

    public async Task<Result> Handle(RegisterAuthenticatorCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Errors.General.NotFound(request.UserId, "User");
        }

        if (_executionContextAccessor.UserId != user.Id)
        {
            return Result.Forbidden(Errors.Authorization.Forbidden("No permission to register authenticator."));
        }

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, request.OtpCode);
        if (!isValid)
        {
            return Errors.Authentication.InvalidTwoFactorAuthenticationToken();
        }

        await _userManager.SetTwoFactorEnabledAsync(user, true);
        return Result.Ok();
    }
}