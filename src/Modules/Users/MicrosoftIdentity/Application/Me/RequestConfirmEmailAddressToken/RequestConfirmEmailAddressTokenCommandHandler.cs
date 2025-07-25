using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.BuildingBlocks.Application.Emails;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.RequestConfirmEmailAddressToken;

internal class RequestConfirmEmailAddressTokenCommandHandler : ICommandHandler<RequestConfirmEmailAddressTokenCommand, Result>
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public RequestConfirmEmailAddressTokenCommandHandler(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IExecutionContextAccessor executionContextAccessor)
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _executionContextAccessor = executionContextAccessor;
    }

    public async Task<Result> Handle(RequestConfirmEmailAddressTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Errors.General.NotFound(request.UserId, "User");
        }

        if (_executionContextAccessor.UserId != user.Id)
        {
            return Result.Forbidden(Errors.Authorization.Forbidden("No permission to request confirm email address token."));
        }

        if (user.Email is null)
        {
            return Errors.General.InvalidRequest("User has no email address");
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _emailSender.SendEmail(new EmailMessage(
            user.Email,
            "MyMeetings - Your Email Confirmation Token",
            $@"You requested a token to confirm your email address. Please use the following token to complete the process: {token}"));

        return Result.Ok();
    }
}