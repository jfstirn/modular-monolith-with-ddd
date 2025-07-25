namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Users;

public class SetUserPermissionsRequest
{
    public IEnumerable<string> Permissions { get; init; } = Enumerable.Empty<string>();
}