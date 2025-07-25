using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Queries;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRoles.ById;

internal class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, Result<RoleDto>>
{
    private readonly RoleManager<Role> _roleManager;

    public GetRolesQueryHandler(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    public Task<Result<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var userRole = (from role in _roleManager.Roles
                        where role.Id == request.RoleId
                        select new RoleDto()
                        {
                            Id = role.Id,
                            Name = role.Name ?? string.Empty
                        }).SingleOrDefault();

        if (userRole is null)
        {
            return Task.FromResult(Result.NotFound<RoleDto>(Errors.General.NotFound(request.RoleId, "User role")));
        }

        return Task.FromResult(Result.Ok(userRole));
    }
}