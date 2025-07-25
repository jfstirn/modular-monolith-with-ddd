using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.SetUserRoles;

public class SetUserRolesCommand : CommandBase<Result>
{
    public SetUserRolesCommand(Guid userId, IEnumerable<Guid> roleIds)
    {
        UserId = userId;
        RoleIds = roleIds;
    }

    public Guid UserId { get; }

    public IEnumerable<Guid> RoleIds { get; }
}