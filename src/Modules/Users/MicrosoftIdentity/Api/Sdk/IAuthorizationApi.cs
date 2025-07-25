using CompanyName.MyMeetings.Modules.UsersMI.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Authorization;
using Refit;

namespace CompanyName.MyMeetings.Modules.UsersMI.Sdk;

public interface IAuthorizationApi
{
    [Get(ApiEndpoints.Authorization.GetPermissions)]
    Task<Result<PermissionsResponse>> GetPermissionsAsync(CancellationToken cancellationToken = default);
}