using Azure.Core;
using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.BuildingBlocks.Application.Emails;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.ChangeEmailAddress;

internal class ChangeEmailAddressCommandHandler : ICommandHandler<ChangeEmailAddressCommand, Result>
{
    private readonly IEmailSender _emailSender;
    private readonly IdentityOptions _identityOptions;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public ChangeEmailAddressCommandHandler(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IExecutionContextAccessor executionContextAccessor, IOptions<IdentityOptions> identityOptions)
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _identityOptions = identityOptions.Value;
        _executionContextAccessor = executionContextAccessor;
    }

    public async Task<Result> Handle(ChangeEmailAddressCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.UserId.ToString());
        if (user is null)
        {
            return Errors.General.NotFound(command.UserId, "User");
        }

        if (_executionContextAccessor.UserId != user.Id)
        {
            return Result.Forbidden(Errors.Authorization.Forbidden("No permission to change email address."));
        }

        if (!Email.IsValid(command.NewEmailAddress, out Error? error))
        {
            return error!;
        }

        var newEmail = Email.Parse(command.NewEmailAddress);

        var result = await _userManager.ChangeEmailAsync(user, newEmail.Address, command.Token);
        if (!result.Succeeded)
        {
            return result.Errors.Select(x => new Error(x.Code, x.Description)).Combine();
        }

        if (_identityOptions.SignIn.RequireConfirmedEmail)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailSender.SendEmail(new EmailMessage(
                command.NewEmailAddress,
                "MyMeetings – Confirm Your Email Address",
                $@"To complete your registration, please use the following token to confirm your email address: {token}"));
        }

        return Result.Ok();
    }
}