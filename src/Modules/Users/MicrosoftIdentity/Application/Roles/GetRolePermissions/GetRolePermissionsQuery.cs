using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRolePermissions;

public class GetRolePermissionsQuery : QueryBase<Result<IEnumerable<PermissionDto>>>
{
    public GetRolePermissionsQuery(Guid roleId)
    {
        RoleId = roleId;
    }

    public Guid RoleId { get; }
}