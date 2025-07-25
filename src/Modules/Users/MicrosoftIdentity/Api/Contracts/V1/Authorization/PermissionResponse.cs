namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Authorization;

public class PermissionResponse
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}