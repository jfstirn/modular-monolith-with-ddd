using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.GetUserAccounts.ById;

public class GetUserAccountsQuery : QueryBase<Result<UserAccountDto>>
{
    public GetUserAccountsQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}