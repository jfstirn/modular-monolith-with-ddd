using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Me.UpdateProfile;

public class UpdateProfileCommand : CommandBase<Result>
{
    public UpdateProfileCommand(Guid userId, string userName, string name, string? firstName, string? lastName)
    {
        UserId = userId;
        UserName = userName;
        Name = name;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid UserId { get; }

    public string UserName { get; }

    public string Name { get; }

    public string? FirstName { get; }

    public string? LastName { get; }
}