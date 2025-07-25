using CompanyName.MyMeetings.Modules.UsersMI.Application.Configuration.Commands;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using Newtonsoft.Json;

namespace CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;

public class CreateUserAccountCommand : InternalCommandBase<Result<Guid>>
{
    [JsonConstructor]
    public CreateUserAccountCommand(Guid id, Guid userId, string login, string? password, string? name, string? firstName, string? lastName, string? emailAddress)
        : base(id)
    {
        UserId = userId;
        Login = login;
        Password = password;
        Name = name;
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
    }

    public Guid UserId { get; }

    public string Login { get; }

    public string? Password { get; }

    public string? Name { get; }

    public string? FirstName { get; }

    public string? LastName { get; }

    public string? EmailAddress { get; }
}