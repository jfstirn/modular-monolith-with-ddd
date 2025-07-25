using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.GetUserAccounts.ById;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.UserAccounts;

[TestFixture]
public class GetUserAccountTests : TestBase
{
    private CreateUserAccountCommand _userAccountCommand = null!;

    [Test]
    public async Task Get_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var getUserQuery = new GetUserAccountsQuery(userId);

        // Act
        var getUserResult = await UserAccessModule.ExecuteQueryAsync(getUserQuery);

        // Assert
        Assert.That(getUserResult.Status, Is.EqualTo(ResultStatus.NotFound));
        Assert.That(getUserResult.IsSuccess, Is.False);
        Assert.That(getUserResult.Value, Is.Null);
    }

    [Test]
    public async Task Get_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var userId = _userAccountCommand.UserId;
        var getUserQuery = new GetUserAccountsQuery(userId);

        // Act
        var getUserResult = await UserAccessModule.ExecuteQueryAsync(getUserQuery);
        var user = getUserResult.Value!;

        // Assert
        Assert.That(getUserResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(getUserResult.IsSuccess, Is.True);
        Assert.That(user, Is.Not.Null);
        Assert.That(user.Id, Is.EqualTo(_userAccountCommand.UserId));
        Assert.That(user.UserName, Is.EqualTo(_userAccountCommand.Login));
        Assert.That(user.Email, Is.EqualTo(_userAccountCommand.EmailAddress));
        Assert.That(user.FirstName, Is.EqualTo(_userAccountCommand.FirstName));
        Assert.That(user.LastName, Is.EqualTo(_userAccountCommand.LastName));
        Assert.That(user.Name, Is.EqualTo(_userAccountCommand.Name));
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _userAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_userAccountCommand);
    }
}