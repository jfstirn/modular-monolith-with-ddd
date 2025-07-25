using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.ChangeEmailAddress;

public class ChangeEmailAddressCommand : CommandBase<Result>
{
    public ChangeEmailAddressCommand(Guid userId, string newEmailAddress, string token)
    {
        UserId = userId;
        NewEmailAddress = newEmailAddress;
        Token = token;
    }

    public Guid UserId { get; }

    public string NewEmailAddress { get; }

    public string Token { get; }
}