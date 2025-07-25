using CompanyName.MyMeetings.BuildingBlocks.Application.Emails;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.ChangeEmailAddress;

internal class ChangeUserEmailAddressCommandHandler : ICommandHandler<ChangeUserEmailAddressCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;

    public ChangeUserEmailAddressCommandHandler(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public async Task<Result> Handle(ChangeUserEmailAddressCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Errors.General.NotFound(request.UserId, "User");
        }

        if (!Email.IsValid(request.NewEmailAddress, out Error? error))
        {
            return error!;
        }

        var newEmail = Email.Parse(request.NewEmailAddress);

        var changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail.Address);
        var result = await _userManager.ChangeEmailAsync(user, newEmail.Address, changeEmailToken);
        if (!result.Succeeded)
        {
            return result.Errors.Map().Combine();
        }

        return Result.Ok();
    }
}