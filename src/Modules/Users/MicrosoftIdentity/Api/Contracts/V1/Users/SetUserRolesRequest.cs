namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Users;

public class SetUserRolesRequest
{
    public IEnumerable<Guid> RoleIds { get; set; } = Enumerable.Empty<Guid>();
}