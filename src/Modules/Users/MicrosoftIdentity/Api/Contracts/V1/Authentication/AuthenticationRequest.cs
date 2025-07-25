using System.ComponentModel.DataAnnotations;

namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Authentication;

public class AuthenticationRequest
{
    public string UserName { get; set; } = null!;

    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}