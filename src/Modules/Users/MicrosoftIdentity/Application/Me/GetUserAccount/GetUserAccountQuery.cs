using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.GetUserAccount;

public class GetUserAccountQuery : QueryBase<Result<UserAccountDto>>
{
    public GetUserAccountQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}