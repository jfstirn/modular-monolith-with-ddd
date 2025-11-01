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
    /// Retrieves a list of user accounts.
    /// </summary>
    /// <remarks>Requires the caller to have the appropriate permission to get the user accounts.</remarks>
    /// <returns>An <see cref="IResult"/> containing the user account directory if the request is authorized and successful;
    /// otherwise, an error result indicating the reason for failure.</returns>
    [HttpGet]
    [HasPermission(UsersPermissions.GetUsers)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
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

        return ToApiResult(response);
    }

    /// <summary>
    /// Retrieves the account details for a specified user.
    /// </summary>
    /// <remarks>Requires the caller to have the appropriate permission to access the account details information.</remarks>
    /// <param name="userId">The unique identifier of the user whose account information is to be retrieved.</param>
    /// <returns>An <see cref="IResult"/> containing the user's account details if found; otherwise, an appropriate error
    /// response.</returns>
    [HttpGet("{userId}")]
    [HasPermission(UsersPermissions.GetUsers)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status404NotFound)]
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

        return ToApiResult(response);
    }

    /// <summary>
    /// Updates the account information for the specified user.
    /// </summary>
    /// <remarks>Requires the caller to have the appropriate permission to update user accounts.</remarks>
    /// <param name="userId">The unique identifier of the user whose account will be updated.</param>
    /// <param name="request">An object containing the updated account details.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the update operation. Returns a success result if the update
    /// is completed; otherwise, returns an error result describing the failure.</returns>
    [HttpPut("{userId}")]
    [HasPermission(UsersPermissions.UpdateUserAccount)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> UpdateUserAccount(Guid userId, UpdateUserAccountRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new UpdateUserAccountCommand(userId, request.Name, request.FirstName, request.LastName));
        return response.ToApiResult();
    }

    /// <summary>
    /// Unlocks the specified user account, allowing the user to regain access if previously locked.
    /// </summary>
    /// <remarks>This operation requires the caller to have the appropriate  permission.</remarks>
    /// <param name="userId">The unique identifier of the user account to unlock.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the unlock operation.</returns>
    [HttpPatch("{userId}/unlock")]
    [HasPermission(UsersPermissions.UnlockUserAccount)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> UnlockUserAccount(Guid userId)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new UnlockUserAccountCommand(userId));
        return response.ToApiResult();
    }

    /// <summary>
    /// Retrieves the list of roles assigned to the specified user.
    /// </summary>
    /// <remarks>This operation requires the caller to have the appropriate permission to access user roles.</remarks>
    /// <param name="userId">The unique identifier of the user whose roles are to be retrieved.</param>
    /// <returns>An <see cref="IResult"/> containing the user's roles if the operation is successful; otherwise, an error result
    /// indicating the reason for failure.</returns>
    [HttpGet("{userId}/roles")]
    [HasPermission(UsersPermissions.GetUserRoles)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status404NotFound)]
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

        return ToApiResult(response);
    }

    /// <summary>
    /// Updates the roles assigned to the specified user.
    /// </summary>
    /// <remarks>This operation requires the caller to have the appropriate permission.</remarks>
    /// <param name="userId">The unique identifier of the user whose roles are to be updated.</param>
    /// <param name="request">An object containing the list of role IDs to assign to the user.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the operation.</returns>
    [HttpPut("{userId}/roles")]
    [HasPermission(UsersPermissions.SetUserRoles)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> SetUserRoles(Guid userId, SetUserRolesRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new SetUserRolesCommand(userId, request.RoleIds));
        return response.ToApiResult();
    }

    /// <summary>
    /// Retrieves the set of permissions assigned to the specified user.
    /// </summary>
    /// <remarks>Requires the caller to have the appropriate permission to access user permissions.</remarks>
    /// <param name="userId">The unique identifier of the user whose permissions are to be retrieved.</param>
    /// <returns>An <see cref="IResult"/> containing the user's permissions if found; otherwise, a result indicating the
    /// appropriate error status.</returns>
    [HttpGet("{userId}/permissions")]
    [HasPermission(UsersPermissions.GetUserPermissions)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status404NotFound)]
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

        return ToApiResult(response);
    }

    /// <summary>
    /// Updates the permissions assigned to the specified user.
    /// </summary>
    /// <remarks>This operation requires the caller to have the appropriate permission.</remarks>
    /// <param name="userId">The unique identifier of the user whose permissions are to be updated.</param>
    /// <param name="request">An object containing the new set of permissions to assign to the user. Cannot be null.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the operation.</returns>
    [HttpPut("{userId}/permissions")]
    [HasPermission(UsersPermissions.SetUserPermissions)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> SetUserPermissions(Guid userId, [FromBody] SetUserPermissionsRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new SetUserPermissionsCommand(userId, request.Permissions));
        return response.ToApiResult();
    }

    /// <summary>
    /// Changes the email address associated with the specified user.
    /// </summary>
    /// <remarks>This operation requires the caller to have the appropriate permission.</remarks>
    /// <param name="userId">The unique identifier of the user whose email address will be updated.</param>
    /// <param name="request">An object containing the new email address to assign to the user. Must not be null.</param>
    /// <returns>An <see cref="IResult"/> indicating the outcome of the operation. Returns a 200 OK result if the email address
    /// was changed successfully; 404 Not Found if the user does not exist; 401 Unauthorized or 403 Forbidden if the
    /// caller lacks sufficient permissions.</returns>
    [HttpPut("{userId}/change-email-address")]
    [HasPermission(UsersPermissions.ChangeUserEmailAddress)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status403Forbidden)]
    public async Task<IResult> ChangeUserEmailAddress(Guid userId, ChangeUserEmailAddressRequest request)
    {
        var response = await _userAccessModule.ExecuteCommandAsync(new ChangeUserEmailAddressCommand(userId, request.NewEmailAddress));
        return response.ToApiResult();
    }
}