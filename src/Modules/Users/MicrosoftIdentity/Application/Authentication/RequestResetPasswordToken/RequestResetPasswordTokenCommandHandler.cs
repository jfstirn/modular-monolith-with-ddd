using CompanyName.MyMeetings.BuildingBlocks.Application.Emails;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.RequestResetPasswordToken;

internal class RequestResetPasswordTokenCommandHandler : ICommandHandler<RequestResetPasswordTokenCommand, Result>
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;

    public RequestResetPasswordTokenCommandHandler(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public async Task<Result> Handle(RequestResetPasswordTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.EmailAddress);
        if (user is not null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailSender.SendEmail(new EmailMessage(
                    request.EmailAddress,
                    "MyMeetings - Your Password Reset Token",
                    $@"You requested a password reset token. Use the following token to proceed:\n\n{token}"));
            return Result.Ok();
        }

        // email user and inform them that they do not have an account with that email address
        var message = new EmailMessage(
            request.EmailAddress,
            "MyMeetings - Forgot password",
            $"We could not find your account with the given email address '{request.EmailAddress}'.\n\nPlease check if you registered with a different email address.");

        await _emailSender.SendEmail(message);
        return Result.Ok();
    }
}