using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.GetUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Me;

[TestFixture]
internal class GetUserAccountTests : TestBase
{
    private CreateUserAccountCommand? _currentUserAccountCommand;
    private CreateUserAccountCommand? _secondUserAccountCommand;

    [Test]
    public async Task GetUserAccount_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var getUserAccountQuery = new GetUserAccountQuery(userId);

        // Act
        var getUserAccountResult = await UserAccessModule.ExecuteQueryAsync(getUserAccountQuery);

        // Assert
        Assert.That(getUserAccountResult.IsSuccess, Is.True);
        Assert.That(getUserAccountResult.Status, Is.EqualTo(ResultStatus.Ok));
    }

    [Test]
    public async Task GetUserAccount_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var getUserAccountQuery = new GetUserAccountQuery(userId);

        // Act
        var getUserAccountResult = await UserAccessModule.ExecuteQueryAsync(getUserAccountQuery);

        // Assert
        Assert.That(getUserAccountResult.IsSuccess, Is.False);
        Assert.That(getUserAccountResult.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task GetUserAccount_ReturnsForbidden_WhenUserIdDoesNotMatchCurrentUser()
    {
        // Arrange
        var userId = _secondUserAccountCommand!.UserId;
        var getUserAccountQuery = new GetUserAccountQuery(userId);

        // Act
        var getUserAccountResult = await UserAccessModule.ExecuteQueryAsync(getUserAccountQuery);

        // Assert
        Assert.That(getUserAccountResult.IsSuccess, Is.False);
        Assert.That(getUserAccountResult.Status, Is.EqualTo(ResultStatus.Forbidden));
        Assert.That(getUserAccountResult.Errors, Is.Not.Empty);
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _currentUserAccountCommand = UserAccountGenerator(userId: CurrentUserId).Generate();
        _secondUserAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_currentUserAccountCommand);
        await UserAccessModule.ExecuteCommandAsync(_secondUserAccountCommand);
    }
}