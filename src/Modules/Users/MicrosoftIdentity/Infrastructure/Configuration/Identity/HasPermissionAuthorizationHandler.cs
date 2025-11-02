using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Authorization.GetPermissions;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.Identity;

internal sealed class HasPermissionAuthorizationHandler
    : AttributeAuthorizationHandler<HasPermissionAuthorizationRequirement, HasPermissionAttribute>
{
    private readonly IUserAccessModule _userAccessModule;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public HasPermissionAuthorizationHandler(
        IUserAccessModule userAccessModule,
        IExecutionContextAccessor executionContextAccessor)
    {
        _userAccessModule = userAccessModule;
        _executionContextAccessor = executionContextAccessor;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HasPermissionAuthorizationRequirement requirement,
        HasPermissionAttribute attribute)
    {
        if (!_executionContextAccessor.IsAvailable)
        {
            context.Fail();
            return;
        }

        if (!TryGetUserId(out var userId))
        {
            context.Fail();
            return;
        }

        var permissions = await GetUserPermissionsAsync(userId);
        if (!HasAdministratorPermission(permissions) || !HasRequiredPermission(attribute.Name, permissions))
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }

    private bool TryGetUserId(out Guid userId)
    {
        try
        {
            userId = _executionContextAccessor.UserId;
            return true;
        }
        catch (ApplicationException)
        {
            userId = default;
            return false;
        }
    }

    private async Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(Guid userId)
    {
        var response = await _userAccessModule.ExecuteQueryAsync(new GetPermissionsQuery(userId));
        return response.IsSuccess
            ? response.Value ?? Enumerable.Empty<PermissionDto>()
            : Enumerable.Empty<PermissionDto>();
    }

    private static bool HasAdministratorPermission(IEnumerable<PermissionDto> permissions) =>
        permissions.Any(x => x.Code.Equals(ApplicationPermissions.Administrator));

    private static bool HasRequiredPermission(string requiredPermission, IEnumerable<PermissionDto> permissions) =>
        permissions.Any(x => x.Code == requiredPermission);
}