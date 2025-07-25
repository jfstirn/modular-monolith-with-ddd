using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Queries;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.AspNetCore.Identity;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.AuthenticatorRegistration.GetAuthenticatorKey;

internal class GetAuthenticatorKeyQueryHandler : IQueryHandler<GetAuthenticatorKeyQuery, Result<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public GetAuthenticatorKeyQueryHandler(UserManager<ApplicationUser> userManager, IExecutionContextAccessor executionContextAccessor)
    {
        _userManager = userManager;
        _executionContextAccessor = executionContextAccessor;
    }

    public async Task<Result<string>> Handle(GetAuthenticatorKeyQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Errors.General.NotFound(request.UserId, "User");
        }

        if (_executionContextAccessor.UserId != user.Id)
        {
            return Result.Forbidden<string>(Errors.Authorization.Forbidden("No permission to get authenticator key."));
        }

        // Try to get the authenticator key from the user
        var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);

        // if none is provided, which means none was generate before
        if (authenticatorKey == null)
        {
            // reset the authenticator key
            await _userManager.ResetAuthenticatorKeyAsync(user);

            // and get it again
            authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        if (!string.IsNullOrEmpty(authenticatorKey))
        {
            return authenticatorKey;
        }

        return Errors.Authentication.AuthenticatorKeyNotFound();
    }
}