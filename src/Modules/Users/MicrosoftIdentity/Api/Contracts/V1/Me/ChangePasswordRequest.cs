using System.ComponentModel.DataAnnotations;

namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Me;

public class ChangePasswordRequest
{
    [DataType(DataType.Password)]
    public required string CurrentPassword { get; init; }

    [DataType(DataType.Password)]
    public required string NewPassword { get; init; }
}