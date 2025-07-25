using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.RequestConfirmEmailAddressToken;

public class RequestConfirmEmailAddressTokenCommand : CommandBase<Result>
{
    public RequestConfirmEmailAddressTokenCommand(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}