namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Authorization;

public class PermissionsResponse
{
    public IEnumerable<PermissionResponse> Permissions { get; set; } = Enumerable.Empty<PermissionResponse>();
}