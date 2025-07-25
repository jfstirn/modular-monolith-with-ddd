using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.GetUserAccounts.Directory;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.UserAccounts;

[TestFixture]
internal class GetUserAccountsTests : TestBase
{
    private CreateUserAccountCommand _firstUserAccountCommand = null!;
    private CreateUserAccountCommand _secondUserAccountCommand = null!;

    [Test]
    public async Task Get_ReturnsAllUsers_WhenUsersExist()
    {
        // Arrange
        var gerUsersQuery = new GetUserAccountsQuery();

        // Act
        var getUsersResult = await UserAccessModule.ExecuteQueryAsync(gerUsersQuery);

        // Assert
        Assert.That(getUsersResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(getUsersResult.Value!.Count, Is.EqualTo(2));
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _firstUserAccountCommand = UserAccountGenerator().Generate();
        _secondUserAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_firstUserAccountCommand);
        await UserAccessModule.ExecuteCommandAsync(_secondUserAccountCommand);
    }
}
