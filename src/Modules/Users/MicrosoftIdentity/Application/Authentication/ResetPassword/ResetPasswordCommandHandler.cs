using CompanyName.MyMeetings.BuildingBlocks.Application.Security;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.ResetPassword;

internal class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.EmailAddress);
        if (user is null)
        {
            return Errors.General.NotFound(request.EmailAddress, "User");
        }

        var newPassword = PasswordManager.HashPassword(request.Password);
        var result = await _userManager.ResetPasswordAsync(user, request.Token, newPassword);
        if (!result.Succeeded)
        {
            return result.Errors.Map().Combine();
        }

        // Check if we have to unlock the user account
        if (await _userManager.IsLockedOutAsync(user))
        {
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
        }

        return Result.Ok();
    }
}