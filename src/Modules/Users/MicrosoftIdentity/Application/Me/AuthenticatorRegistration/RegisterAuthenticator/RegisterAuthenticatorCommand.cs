using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.AuthenticatorRegistration.RegisterAuthenticator;

public class RegisterAuthenticatorCommand : CommandBase<Result>
{
    public RegisterAuthenticatorCommand(Guid userId, string otpCode)
    {
        UserId = userId;
        OtpCode = otpCode;
    }

    public Guid UserId { get; }

    public string OtpCode { get; }
}