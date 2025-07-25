using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.Login;

public class AccountLoginCommand : CommandBase<AuthenticationResult>
{
    public AccountLoginCommand(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public string Login { get; }

    public string Password { get; }
}