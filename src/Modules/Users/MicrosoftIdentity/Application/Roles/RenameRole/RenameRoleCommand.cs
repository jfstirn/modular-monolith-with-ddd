using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.RenameRole;

public class RenameRoleCommand : CommandBase<Result>
{
    public RenameRoleCommand(Guid roleId, string name)
    {
        RoleId = roleId;
        Name = name;
    }

    public Guid RoleId { get; }

    public string Name { get; }
}