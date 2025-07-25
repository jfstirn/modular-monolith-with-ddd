using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.SetRolePermissions;

public class SetRolePermissionsCommand : CommandBase<Result>
{
    public SetRolePermissionsCommand(Guid roleId, IEnumerable<string> permissions)
    {
        RoleId = roleId;
        Permissions = permissions;
    }

    public Guid RoleId { get; }

    public IEnumerable<string> Permissions { get; }
}