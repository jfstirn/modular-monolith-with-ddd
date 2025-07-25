using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Authorization.GetPermissions;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.ChangeEmailAddress;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.GetUserRoles;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.SetUserPermissions;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.SetUserRoles;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.UnlockUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.UpdateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersApplication = CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.GetUserAccounts;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Users;

[Route("api/users/accounts")]
public class UsersController : ApplicationController
{
    private readonly IUserAccessModule _userAccessModule;

    public UsersController(IUserAccessModule userAccessModule)
    {
        _userAccessModule = userAccessModule;
    }

    /// <summary>
    /// Gets the user directory.
    /// </summary>
    /// <returns>List of users.</returns>
    [HttpGet]
    [HasPermission(UsersPermissions.GetUsers)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> GetUserAccountDirectory()
    {
        var response = await _userAccessModule.ExecuteQueryAsync(new UsersApplication.Directory.GetUserAccountsQuery());
        if (response.IsSuccess && response.Value is not null)
        {
            var userAccountsResponse = new UserAccountsResponse
            {
                UserAccounts = response.Value.Select(r => new UserAccountResponse
                {
                    Id = r.Id,
                    Name = r.Name,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    Login = r.UserName,
                    NormalizedLogin = r.NormalizedUserName,
                    LockoutEnd = r.LockoutEnd,
                    LockoutEnabled = r.LockoutEnabled,
                    AccessFailedCount = r.AccessFailedCount,
                    TwoFactorEnabled = r.TwoFactorEnabled,
                    PhoneNumber = r.PhoneNumber,
                    PhoneNumberConfirmed = r.PhoneNumberConfirmed,
                    Email = r.Email,
                    NormalizedEmail = r.NormalizedEmail,
                    EmailConfirmed = r.EmailConfirmed
                }).ToList()
            };

            return response.ToApiResult(userAccountsResponse);
        }

        return FromResponse(response);
    }

    [HttpGet("{userId}")]
    [HasPermission(UsersPermissions.GetUsers)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> GetUserAccount(Guid userId)
    {
        var response = await _userAccessModule.ExecuteQueryAsync(new UsersApplication.ById.GetUserAccountsQuery(userId));
        if (response.IsSuccess && response.Value is not null)
        {
            var userAccountResponse = new UserAccountResponse
            {
                Id = response.Value.Id,
                Name = response.Value.Name,
                FirstName = response.Value.FirstName,
                LastName = response.Value.LastName,
                Login = response.Value.UserName,
                NormalizedLogin = response.Value.NormalizedUserName,
                LockoutEnd = response.Value.LockoutEnd,
                LockoutEnabled = response.Value.LockoutEnabled,
                AccessFailedCount = response.Value.AccessFailedCount,
                TwoFactorEnabled = response.Value.TwoFactorEnabled,
                PhoneNumber = response.Value.PhoneNumber,
                PhoneNumberConfirmed = response.Value.PhoneNumberConfirmed,
                Email = response.Value.Email,
                NormalizedEmail = response.Value.NormalizedEmail,
                EmailConfirmed = response.Value.EmailConfirmed
            };

            return response.ToApiResult(userAccountResponse);
        }

        return FromResponse(response);
    }

    [HttpPut("{userId}")]
    [HasPermission(UsersPermissions.UpdateUserAccount)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> UpdateUserAccount(Guid userId, UpdateUserAccountRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new UpdateUserAccountCommand(userId, request.Name, request.FirstName, request.LastName));
        return response.ToApiResult();
    }

    [HttpPut("{userId}/unlock")]
    [HasPermission(UsersPermissions.UnlockUserAccount)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> UnlockUserAccount(Guid userId)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new UnlockUserAccountCommand(userId));
        return response.ToApiResult();
    }

    [HttpGet("{userId}/roles")]
    [HasPermission(UsersPermissions.GetUserRoles)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> GetUserRoles(Guid userId)
    {
        var response = await _userAccessModule.ExecuteQueryAsync(new GetUserRolesQuery(userId));
        if (response.IsSuccess && response.Value is not null)
        {
            var userRolesResponse = new RolesResponse
            {
                Roles = response.Value.Select(r => new RoleResponse
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToList()
            };

            return response.ToApiResult(userRolesResponse);
        }

        return FromResponse(response);
    }

    [HttpPut("{userId}/roles")]
    [HasPermission(UsersPermissions.SetUserRoles)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> SetUserRoles(Guid userId, SetUserRolesRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new SetUserRolesCommand(userId, request.RoleIds));
        return response.ToApiResult();
    }

    [HttpGet("{userId}/permissions")]
    [HasPermission(UsersPermissions.GetUserPermissions)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> GetUserPermissions(Guid userId)
    {
        var response = await _userAccessModule.ExecuteQueryAsync(new GetPermissionsQuery(userId));
        if (response.IsSuccess && response.Value is not null)
        {
            var userPermissionsResponse = new PermissionsResponse
            {
                Permissions = response.Value.Select(r => new PermissionResponse
                {
                    Code = r.Code,
                    Name = r.Name,
                    Description = r.Description
                }).ToList()
            };

            return response.ToApiResult(userPermissionsResponse);
        }

        return FromResponse(response);
    }

    [HttpPut("{userId}/permissions")]
    [HasPermission(UsersPermissions.SetUserPermissions)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> SetUserPermissions(Guid userId, [FromBody] SetUserPermissionsRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new SetUserPermissionsCommand(userId, request.Permissions));
        return response.ToApiResult();
    }

    [HttpPut("{userId}/change-email-address")]
    [HasPermission(UsersPermissions.ChangeUserEmailAddress)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> ChangeUserEmailAddress(Guid userId, ChangeUserEmailAddressRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new ChangeUserEmailAddressCommand(userId, request.NewEmailAddress));
        return response.ToApiResult();
    }
}