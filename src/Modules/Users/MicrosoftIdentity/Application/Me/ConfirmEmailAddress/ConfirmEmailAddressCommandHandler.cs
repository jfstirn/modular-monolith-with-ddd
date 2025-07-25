using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.ConfirmEmailAddress;

internal class ConfirmEmailAddressCommandHandler : ICommandHandler<ConfirmEmailAddressCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public ConfirmEmailAddressCommandHandler(UserManager<ApplicationUser> userManager, IExecutionContextAccessor executionContextAccessor)
    {
        _userManager = userManager;
        _executionContextAccessor = executionContextAccessor;
    }

    public async Task<Result> Handle(ConfirmEmailAddressCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Errors.General.NotFound(request.UserId, "User");
        }

        if (_executionContextAccessor.UserId != user.Id)
        {
            return Result.Forbidden(Errors.Authorization.Forbidden("No permission to confirm email address."));
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        {
            return result.Errors.Map().Combine();
        }

        return Result.Ok();
    }
}