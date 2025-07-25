namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRolePermissions;

public class PermissionDto
{
    public PermissionDto(string code, string name, string? description)
    {
        Code = code;
        Name = name;
        Description = description;
    }

    public string Code { get; }

    public string Name { get; }

    public string? Description { get; }
}