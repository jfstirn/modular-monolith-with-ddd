using System.Security.Claims;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Queries;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using CompanyName.MyMeetings.Modules.UsersMI.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Authorization.GetPermissions;

internal class GetUserPermissionsQueryHandler : IQueryHandler<GetPermissionsQuery, Result<IEnumerable<PermissionDto>>>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IReadOnlyPermissionRepository _permissionRepository;

    public GetUserPermissionsQueryHandler(
            IReadOnlyPermissionRepository permissionRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _permissionRepository = permissionRepository;
    }

    public async Task<Result<IEnumerable<PermissionDto>>> Handle(GetPermissionsQuery query, CancellationToken cancellationToken)
    {
        var options = query.MapToOptions();

        if (query.UserId is not null)
        {
            var user = await _userManager.FindByIdAsync(query.UserId.Value.ToString());
            if (user is null)
            {
                return Errors.General.NotFound(query.UserId.Value, "User");
            }

            var roleNames = await _userManager.GetRolesAsync(user);
            var roleClaims = await GetClaimsAsync(roleNames);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var claims = roleClaims.Union(userClaims)
                .Where(x => x.Type == CustomClaimTypes.Permission)
                .Select(x => x.Value)
                .ToList();

            // Short circuit
            if (!claims.Any())
            {
                return Result.Ok(Enumerable.Empty<PermissionDto>());
            }

            options = options.WithCodes(claims);
        }

        var permissions = await _permissionRepository.GetPermissionsAsync(options, cancellationToken);
        var result = permissions.Select(p => new PermissionDto(p.Code, p.Name, p.Description)).ToList();
        return result;
    }

    private async Task<List<Claim>> GetClaimsAsync(IEnumerable<string> roleNames)
    {
        var claims = new List<Claim>();
        foreach (var roleName in roleNames)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null)
            {
                continue;
            }

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            claims.AddRange(roleClaims ?? Enumerable.Empty<Claim>());
        }

        return claims;
    }
}