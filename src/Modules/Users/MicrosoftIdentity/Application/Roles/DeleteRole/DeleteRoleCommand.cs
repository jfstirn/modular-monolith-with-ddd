using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.DeleteRole;

public class DeleteRoleCommand : CommandBase<Result>
{
    public DeleteRoleCommand(Guid roleId)
    {
        RoleId = roleId;
    }

    public Guid RoleId { get; }
}