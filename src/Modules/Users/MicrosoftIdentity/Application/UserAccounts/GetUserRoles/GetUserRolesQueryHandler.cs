using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Queries;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.GetUserRoles;

internal class GetUserRolesQueryHandler : IQueryHandler<GetUserRolesQuery, Result<IEnumerable<RoleDto>>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public GetUserRolesQueryHandler(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result<IEnumerable<RoleDto>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Errors.General.NotFound(request.UserId, "User");
        }

        var roleNames = await _userManager.GetRolesAsync(user);
        if (!roleNames.Any())
        {
            return Result.Ok(Enumerable.Empty<RoleDto>());
        }

        var roles = (from role in _roleManager.Roles
                     where roleNames.Contains(role.Name ?? string.Empty)
                     select new RoleDto()
                     {
                         Id = role.Id,
                         Name = role.Name!
                     }).ToList();

        return Result.Ok(roles.AsEnumerable());
    }
}