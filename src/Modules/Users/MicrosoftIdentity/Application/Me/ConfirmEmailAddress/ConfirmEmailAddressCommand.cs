using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.ConfirmEmailAddress;

public class ConfirmEmailAddressCommand : CommandBase<Result>
{
    public ConfirmEmailAddressCommand(Guid userId, string token)
    {
        Token = token;
        UserId = userId;
    }

    public Guid UserId { get; }

    public string Token { get; }
}