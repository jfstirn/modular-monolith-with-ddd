using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Queries;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.GetUserAccount;

internal class GetUserAccountQueryHandler : IQueryHandler<GetUserAccountQuery, Result<UserAccountDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public GetUserAccountQueryHandler(UserManager<ApplicationUser> userManager, IExecutionContextAccessor executionContextAccessor)
    {
        _userManager = userManager;
        _executionContextAccessor = executionContextAccessor;
    }

    public async Task<Result<UserAccountDto>> Handle(GetUserAccountQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Errors.General.NotFound(request.UserId, "user");
        }

        if (_executionContextAccessor.UserId != user.Id)
        {
            return Result.Forbidden<UserAccountDto>(Errors.Authorization.Forbidden("No permission to get user account."));
        }

        return new UserAccountDto
        {
            Id = user.Id,
            IsActive = user.LockoutEnd is null,
            EmailAddress = user.Email,
            UserName = user.UserName,
            Name = user.Name,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}