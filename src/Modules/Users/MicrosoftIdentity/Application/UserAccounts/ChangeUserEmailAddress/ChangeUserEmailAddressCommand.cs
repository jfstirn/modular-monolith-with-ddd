using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.ChangeEmailAddress;

public class ChangeUserEmailAddressCommand : CommandBase<Result>
{
    public ChangeUserEmailAddressCommand(Guid userId, string newEmailAddress)
    {
        UserId = userId;
        NewEmailAddress = newEmailAddress;
    }

    public Guid UserId { get; }

    public string NewEmailAddress { get; }
}