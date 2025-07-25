namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Roles;

public class RolesResponse
{
    public IEnumerable<RoleResponse> Roles { get; set; } = Enumerable.Empty<RoleResponse>();
}