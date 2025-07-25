using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.UpdateProfile;

internal class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public UpdateProfileCommandHandler(UserManager<ApplicationUser> userManager, IExecutionContextAccessor executionContextAccessor)
    {
        _userManager = userManager;
        _executionContextAccessor = executionContextAccessor;
    }

    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var userById = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (userById is null)
        {
            return Errors.General.NotFound(request.UserId, "User");
        }

        if (_executionContextAccessor.UserId != userById.Id)
        {
            return Result.Forbidden(Errors.Authorization.Forbidden("No permission to update profile."));
        }

        var userName = request.UserName.Trim();
        var userByUserName = await _userManager.FindByNameAsync(userName);
        if (userByUserName is not null && userByUserName.Id != userById.Id)
        {
            return Errors.General.ValueMustBeUnique($"UserName '{request.UserName}' already taken");
        }

        var user = userById;
        user.UserName = userName;
        user.Name = request.Name.Trim();
        user.FirstName = request.FirstName?.Trim();
        user.LastName = request.LastName?.Trim();

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return result.Errors.Select(x => new Error(x.Code, x.Description)).Combine();
        }

        return Result.Ok();
    }
}