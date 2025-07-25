using CompanyName.MyMeetings.Modules.UsersMI.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Roles;
using Refit;

namespace CompanyName.MyMeetings.Modules.UsersMI.Sdk;

public interface IRoleApi
{
    [Get(ApiEndpoints.Roles.GetRoles)]
    Task<Result<RolesResponse>> GetRolesAsync(CancellationToken cancellationToken = default);

    [Get(ApiEndpoints.Roles.GetRoleById)]
    Task<Result<RoleResponse>> GetRoleAsync(Guid roleId, CancellationToken cancellationToken = default);

    [Post(ApiEndpoints.Roles.AddRole)]
    Task<Result<Guid?>> AddRoleAsync(AddRoleRequest request, CancellationToken cancellationToken = default);

    [Put(ApiEndpoints.Roles.RenameRole)]
    Task<Result> RenameRoleAsync(Guid roleId, RenameRoleRequest request, CancellationToken cancellationToken = default);

    [Delete(ApiEndpoints.Roles.DeleteRole)]
    Task<Result> DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default);

    [Get(ApiEndpoints.Roles.GetRolePermissions)]
    Task<Result<PermissionsResponse>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default);

    [Put(ApiEndpoints.Roles.SetRolePermissions)]
    Task<Result> SetRolePermissionsAsync(Guid roleId, SetRolePermissionsRequest request, CancellationToken cancellationToken = default);
}