namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Roles;

public class AddRoleRequest
{
    public required string Name { get; init; }

    public required string[] Permissions { get; init; }
}