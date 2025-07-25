using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Queries;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRoles.Directory;

internal class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, Result<IEnumerable<RoleDto>>>
{
    private readonly RoleManager<Role> _roleManager;

    public GetRolesQueryHandler(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    public Task<Result<IEnumerable<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = (from role in _roleManager.Roles
                     select new RoleDto()
                     {
                         Id = role.Id,
                         Name = role.Name ?? string.Empty
                     }).ToList();

        return Task.FromResult(Result.Ok(roles.AsEnumerable()));
    }
}