using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.RequestChangeEmailAddressToken
{
    public class RequestChangeEmailAddressTokenCommand : CommandBase<Result>
    {
        public RequestChangeEmailAddressTokenCommand(Guid userId, string newEmailAddress)
        {
            UserId = userId;
            NewEmailAddress = newEmailAddress;
        }

        public Guid UserId { get; }

        public string NewEmailAddress { get; }
    }
}