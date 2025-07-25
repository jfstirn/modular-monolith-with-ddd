namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Roles;

public class PermissionsResponse
{
    public IEnumerable<PermissionResponse> Permissions { get; set; } = Enumerable.Empty<PermissionResponse>();
}