using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.UnlockUserAccount;

public class UnlockUserAccountCommand : CommandBase<Result>
{
    public UnlockUserAccountCommand(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}