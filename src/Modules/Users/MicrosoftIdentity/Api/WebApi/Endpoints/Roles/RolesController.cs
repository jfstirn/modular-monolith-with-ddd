using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.CreateRole;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.DeleteRole;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRolePermissions;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.RenameRole;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.SetRolePermissions;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolesApplication = CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRoles;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Roles;

[Route("api/users/roles")]
public class RolesController : ApplicationController
{
    private readonly IUserAccessModule _userAccessModule;

    public RolesController(IUserAccessModule userAccessModule)
    {
        _userAccessModule = userAccessModule;
    }

    /// <summary>
    /// Retrieves a list of all roles.
    /// </summary>
    /// <remarks>Requires the caller to have the appropriate permission to access role information.</remarks>
    /// <returns>An <see cref="IResult"/> containing the collection of roles if the operation is successful; otherwise, an error
    /// result describing the failure.</returns>
    [HttpGet]
    [HasPermission(UsersPermissions.GetRoles)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> GetRoles()
    {
        var response = await _userAccessModule.ExecuteQueryAsync(new RolesApplication.Directory.GetRolesQuery());
        if (response.IsSuccess && response.Value is not null)
        {
            var rolesResponse = new RolesResponse
            {
                Roles = response.Value.Select(r => new RoleResponse
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToList()
            };

            return response.ToApiResult(rolesResponse);
        }

        return ToApiResult(response);
    }

    /// <summary>
    /// Retrieves the details of a role.
    /// </summary>
    /// <remarks>Requires the caller to have the appropriate permission to access role information.</remarks>
    /// <param name="roleId">The unique identifier of the role to retrieve.</param>
    /// <returns>An <see cref="IResult"/> containing the role details if found; otherwise, an error result indicating the reason
    /// for failure.</returns>
    [HttpGet("{roleId}")]
    [HasPermission(UsersPermissions.GetRoles)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> GetRole(Guid roleId)
    {
        var response = await _userAccessModule.ExecuteQueryAsync(new RolesApplication.ById.GetRolesQuery(roleId));
        if (response.IsSuccess && response.Value is not null)
        {
            var roleResponse = new RoleResponse
            {
                Id = response.Value.Id,
                Name = response.Value.Name
            };

            return response.ToApiResult(roleResponse);
        }

        return ToApiResult(response);
    }

    /// <summary>
    /// Creates a new user role with the specified name and permissions.
    /// </summary>
    /// <remarks>Requires the caller to have the appropriate permission to create a role.</remarks>
    /// <param name="request">An object containing the details of the role to add, including the role name and associated permissions.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the operation. Returns a success result if the role is
    /// created; otherwise, returns an error result describing the failure.</returns>
    [HttpPost]
    [HasPermission(UsersPermissions.AddRole)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> AddRole([FromBody] AddRoleRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new CreateRoleCommand(request.Name, request.Permissions));
        return response.ToApiResult();
    }

    /// <summary>
    /// Renames an existing user role.
    /// </summary>
    /// <remarks>Requires the caller to have the appropriate permission to rename the role.</remarks>
    /// <param name="roleId">The unique identifier of the role to rename.</param>
    /// <param name="request">An object containing the new name for the role. The name must meet any validation requirements defined by the
    /// system.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the operation.</returns>
    [HttpPut("{roleId}/rename")]
    [HasPermission(UsersPermissions.RenameRole)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> RenameRole(Guid roleId, [FromBody] RenameRoleRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new RenameRoleCommand(roleId, request.Name));
        return response.ToApiResult();
    }

    /// <summary>
    /// Deletes the role.
    /// </summary>
    /// <remarks>Requires the caller to have the appropriate permission to delete the role.</remarks>
    /// <param name="roleId">The unique identifier of the role to delete.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the delete operation.</returns>
    [HttpDelete("{roleId}")]
    [HasPermission(UsersPermissions.DeleteRole)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> DeleteRole(Guid roleId)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new DeleteRoleCommand(roleId));
        return response.ToApiResult();
    }

    /// <summary>
    /// Retrieves the list of permissions assigned to the specified role.
    /// </summary>
    /// <remarks>Requires the caller to have the appropriate permission to access role permissions.</remarks>
    /// <param name="roleId">The unique identifier of the role for which to retrieve permissions.</param>
    /// <returns>An <see cref="IResult"/> containing the permissions for the specified role if found; otherwise, a result indicating the error.</returns>
    [HttpGet("{roleId}/permissions")]
    [HasPermission(UsersPermissions.GetRolePermissions)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> GetRolePermissions(Guid roleId)
    {
        var response = await _userAccessModule.ExecuteQueryAsync(new GetRolePermissionsQuery(roleId));
        if (response.IsSuccess && response.Value is not null)
        {
            var permissionsResponse = new PermissionsResponse
            {
                Permissions = response.Value.Select(r => new PermissionResponse
                {
                    Code = r.Code,
                    Name = r.Name,
                    Description = r.Description
                }).ToList()
            };
            return response.ToApiResult(permissionsResponse);
        }

        return ToApiResult(response);
    }

    /// <summary>
    /// Updates the set of permissions assigned to the specified role.
    /// </summary>
    /// <remarks>Requires the caller to have the appropriate permission to modify role permissions.</remarks>
    /// <param name="roleId">The unique identifier of the role whose permissions are to be updated.</param>
    /// <param name="request">An object containing the new set of permissions to assign to the role.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the operation.</returns>
    [HttpPut("{roleId}/permissions")]
    [HasPermission(UsersPermissions.SetRolePermissions)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> SetRolePermissions(Guid roleId, [FromBody] SetRolePermissionsRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new SetRolePermissionsCommand(roleId, request.Permissions));
        return response.ToApiResult();
    }
}