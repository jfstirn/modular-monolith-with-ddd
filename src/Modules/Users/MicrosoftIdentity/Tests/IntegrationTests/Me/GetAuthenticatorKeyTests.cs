using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.AuthenticatorRegistration.GetAuthenticatorKey;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Me;

[TestFixture]
internal class GetAuthenticatorKeyTests : TestBase
{
    private CreateUserAccountCommand? _currentUserAccountCommand;
    private CreateUserAccountCommand? _secondUserAccountCommand;

    [Test]
    public async Task GetAuthenticatorKey_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var getAuthenticatorKeyQuery = new GetAuthenticatorKeyQuery(userId);

        // Act
        var getAuthenticatorKeyResult = await UserAccessModule.ExecuteQueryAsync(getAuthenticatorKeyQuery);

        // Assert
        Assert.That(getAuthenticatorKeyResult.IsSuccess, Is.False);
        Assert.That(getAuthenticatorKeyResult.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task GetAuthenticatorKey_ReturnsOk_WhenDataIsValid()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var getAuthenticatorKeyQuery = new GetAuthenticatorKeyQuery(userId);

        // Act
        var getAuthenticatorKeyResult = await UserAccessModule.ExecuteQueryAsync(getAuthenticatorKeyQuery);

        // Assert
        Assert.That(getAuthenticatorKeyResult.IsSuccess, Is.True);
        Assert.That(getAuthenticatorKeyResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(getAuthenticatorKeyResult.Value, Is.Not.Null);
    }

    [Test]
    public async Task GetAuthenticatorKey_ReturnsForbidden_WhenUserIdDoesNotMatchCurrentUser()
    {
        // Arrange
        var userId = _secondUserAccountCommand!.UserId;
        var getAuthenticatorKeyQuery = new GetAuthenticatorKeyQuery(userId);

        // Act
        var getAuthenticatorKeyResult = await UserAccessModule.ExecuteQueryAsync(getAuthenticatorKeyQuery);

        // Assert
        Assert.That(getAuthenticatorKeyResult.IsSuccess, Is.False);
        Assert.That(getAuthenticatorKeyResult.Status, Is.EqualTo(ResultStatus.Forbidden));
        Assert.That(getAuthenticatorKeyResult.Errors, Is.Not.Empty);
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _currentUserAccountCommand = UserAccountGenerator(userId: CurrentUserId).Generate();
        _secondUserAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_currentUserAccountCommand);
        await UserAccessModule.ExecuteCommandAsync(_secondUserAccountCommand);
    }
}