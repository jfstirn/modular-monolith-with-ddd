namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Users;

public class PermissionResponse
{
    public required string Code { get; init; }

    public required string Name { get; init; }

    public string? Description { get; set; }
}