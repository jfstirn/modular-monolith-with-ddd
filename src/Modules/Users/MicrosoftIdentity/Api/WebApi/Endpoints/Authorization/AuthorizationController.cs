using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthorizationApplication = CompanyName.MyMeetings.Modules.UsersMI.Application.Authorization.GetPermissions;

namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints.Authorization;

[Route("api/authorization")]
public class AuthorizationController : ApplicationController
{
    private readonly IUserAccessModule _userAccessModule;

    public AuthorizationController(IUserAccessModule userAccessModule)
    {
        _userAccessModule = userAccessModule;
    }

    /// <summary>
    /// Retrieves a directory of available permissions for the application.
    /// </summary>
    /// <remarks>The returned permission can be assigned to roles or users to control access to various features.</remarks>
    /// <returns>An <see cref="IResult"/> containing the list of permissions if successful; otherwise, an error result indicating
    /// the reason for failure.</returns>
    [HttpGet("permissions")]
    [NoPermissionRequired]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IResult), StatusCodes.Status401Unauthorized)]
    public async Task<IResult> GetPermissionDirectory()
    {
        var response = await _userAccessModule.ExecuteQueryAsync(new AuthorizationApplication.GetPermissionsQuery(null));
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
}