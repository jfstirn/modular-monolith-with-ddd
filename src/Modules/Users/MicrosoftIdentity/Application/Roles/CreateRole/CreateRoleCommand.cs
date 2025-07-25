using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.CreateRole;

public class CreateRoleCommand : CommandBase<Result<Guid>>
{
    public CreateRoleCommand(string name, IEnumerable<string>? permissions)
    {
        Name = name;
        Permissions = permissions;
    }

    public string Name { get; }

    public IEnumerable<string>? Permissions { get; }
}