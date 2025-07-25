using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.ChangePassword;

public class ChangePasswordCommand : CommandBase<Result>
{
    public ChangePasswordCommand(Guid userId, string currentPassword, string newPassword)
    {
        UserId = userId;
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
    }

    public Guid UserId { get; }

    public string CurrentPassword { get; }

    public string NewPassword { get; }
}