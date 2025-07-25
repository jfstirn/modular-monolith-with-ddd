namespace CompanyName.MyMeetings.Modules.UsersMI.Contracts.V1.Users;

public class UserAccountsResponse
{
    public IEnumerable<UserAccountResponse> UserAccounts { get; init; } = Enumerable.Empty<UserAccountResponse>();
}