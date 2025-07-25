using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.SetUserPermissions;

public class SetUserPermissionsCommand : CommandBase<Result>
{
    public SetUserPermissionsCommand(Guid userId, IEnumerable<string> permissions)
    {
        UserId = userId;
        Permissions = permissions;
    }

    public Guid UserId { get; }

    public IEnumerable<string> Permissions { get; }
}