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
using UserRoleContracts = CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Roles;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Roles;

[Route("api/users/roles")]
public class RolesController : ApplicationController
{
    private readonly IUserAccessModule _userAccessModule;

    public RolesController(IUserAccessModule userAccessModule)
    {
        _userAccessModule = userAccessModule;
    }

    [HttpGet]
    [HasPermission(UsersPermissions.GetRoles)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> GetRoleDirectory()
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

        return FromResponse(response);
    }

    [HttpGet("{roleId}")]
    [HasPermission(UsersPermissions.GetRoles)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
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

        return FromResponse(response);
    }

    [HttpPost]
    [HasPermission(UsersPermissions.AddRole)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> AddRole([FromBody] UserRoleContracts.AddRoleRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new CreateRoleCommand(request.Name, request.Permissions));
        return response.ToApiResult();
    }

    [HttpPatch("{roleId}/rename")]
    [HasPermission(UsersPermissions.RenameRole)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> RenameRole(Guid roleId, [FromBody] UserRoleContracts.RenameRoleRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new RenameRoleCommand(roleId, request.Name));
        return response.ToApiResult();
    }

    [HttpDelete("{roleId}")]
    [HasPermission(UsersPermissions.DeleteRole)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> DeleteRole(Guid roleId)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new DeleteRoleCommand(roleId));
        return response.ToApiResult();
    }

    [HttpGet("{roleId}/permissions")]
    [HasPermission(UsersPermissions.GetRolePermissions)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
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

        return FromResponse(response);
    }

    [HttpPatch("{roleId}/permissions")]
    [HasPermission(UsersPermissions.SetRolePermissions)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> SetRolePermissions(Guid roleId, [FromBody] UserRoleContracts.SetRolePermissionsRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new SetRolePermissionsCommand(roleId, request.Permissions));
        return response.ToApiResult();
    }
}