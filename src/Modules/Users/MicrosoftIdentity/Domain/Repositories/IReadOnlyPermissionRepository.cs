namespace CompanyName.MyMeetings.Modules.UsersMI.Domain.Repositories;

public interface IReadOnlyPermissionRepository
{
    Task<IEnumerable<Permission>> GetPermissionsAsync(GetPermissionsOptions options, CancellationToken cancellationToken = default);
}

public record GetPermissionsOptions(IEnumerable<string>? PermissionCodes);