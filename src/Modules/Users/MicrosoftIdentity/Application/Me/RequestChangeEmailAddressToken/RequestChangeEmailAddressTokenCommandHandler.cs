using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.BuildingBlocks.Application.Emails;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.RequestChangeEmailAddressToken;

internal class RequestChangeEmailAddressTokenCommandHandler : ICommandHandler<RequestChangeEmailAddressTokenCommand, Result>
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public RequestChangeEmailAddressTokenCommandHandler(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IExecutionContextAccessor executionContextAccessor)
    {
        _emailSender = emailSender;
        _userManager = userManager;
        _executionContextAccessor = executionContextAccessor;
    }

    public async Task<Result> Handle(RequestChangeEmailAddressTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Errors.General.NotFound(request.UserId, "User");
        }

        if (_executionContextAccessor.UserId != user.Id)
        {
            return Result.Forbidden(Errors.Authorization.Forbidden("No permission to request change email address token."));
        }

        if (user.Email is null)
        {
            return Errors.General.InvalidRequest("User has no email address");
        }

        var token = await _userManager.GenerateChangeEmailTokenAsync(user, request.NewEmailAddress);
        await _emailSender.SendEmail(new EmailMessage(
            user.Email,
            "MyMeetings - Your Email Change Token",
            $"You requested to change your email address. Use the following token to confirm the change: {token}"));

        return Result.Ok();
    }
}