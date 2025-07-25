using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.AuthenticatorRegistration.GetAuthenticatorKey;

public class GetAuthenticatorKeyQuery : QueryBase<Result<string>>
{
    public GetAuthenticatorKeyQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}