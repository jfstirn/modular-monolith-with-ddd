using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRoles.ById;

public class GetRolesQuery : QueryBase<Result<RoleDto>>
{
    public GetRolesQuery(Guid roleId)
    {
        RoleId = roleId;
    }

    public Guid RoleId { get; }
}