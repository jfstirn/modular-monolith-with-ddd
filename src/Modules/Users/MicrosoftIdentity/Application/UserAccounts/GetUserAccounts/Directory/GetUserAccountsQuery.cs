using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.GetUserAccounts.Directory;

public class GetUserAccountsQuery : QueryBase<Result<IEnumerable<UserAccountDto>>>
{
}