using CompanyName.MyMeetings.Modules.UsersMI.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Users;
using Refit;

namespace CompanyName.MyMeetings.Modules.UsersMI.Sdk;

public interface IUserApi
{
    [Get(ApiEndpoints.Users.GetUsers)]
    Task<Result<UserAccountsResponse>> GetUserAccountsAsync(CancellationToken cancellationToken = default);

    [Get(ApiEndpoints.Users.GetUserById)]
    Task<Result<UserAccountResponse>> GetUserAccountAsync(Guid userId, CancellationToken cancellationToken = default);

    [Put(ApiEndpoints.Users.UpdateUser)]
    Task<Result> UpdateUserAccountAsync(Guid userId, UpdateUserAccountRequest request, CancellationToken cancellationToken = default);

    [Put(ApiEndpoints.Users.UnlockUser)]
    Task<Result> UnlockUserAccountAsync(Guid userId, CancellationToken cancellationToken = default);

    [Get(ApiEndpoints.Users.GetUserRoles)]
    Task<Result<RolesResponse>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);

    [Put(ApiEndpoints.Users.SetUserRoles)]
    Task<Result> SetUserRolesAsync(Guid userId, SetUserRolesRequest request, CancellationToken cancellationToken = default);

    [Get(ApiEndpoints.Users.GetUserPermissions)]
    Task<Result<PermissionsResponse>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);

    [Put(ApiEndpoints.Users.SetUserPermissions)]
    Task<Result> SetUserPermissionsAsync(Guid userId, SetUserPermissionsRequest request, CancellationToken cancellationToken = default);

    [Put(ApiEndpoints.Users.ChangeUserEmailAddress)]
    Task<Result> ChangeUserEmailAddressAsync(Guid userId, ChangeUserEmailAddressRequest request, CancellationToken cancellationToken = default);
}