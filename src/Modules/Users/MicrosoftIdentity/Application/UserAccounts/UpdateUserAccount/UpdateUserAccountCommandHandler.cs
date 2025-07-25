using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.UpdateUserAccount;

internal class UpdateUserAccountCommandHandler : ICommandHandler<UpdateUserAccountCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUserAccountCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(UpdateUserAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Errors.General.NotFound(request.UserId, "User");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Errors.General.ValueIsRequired(nameof(request.Name));
        }

        user.Name = request.Name.Trim();
        user.FirstName = request.FirstName?.Trim();
        user.LastName = request.LastName?.Trim();

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return result.Errors.Map().Combine();
        }

        return Result.Ok();
    }
}