namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Roles;

public class SetRolePermissionsRequest
{
    public required string[] Permissions { get; init; }
}