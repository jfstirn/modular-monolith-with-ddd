using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.AuthenticatorRegistration.GetAuthenticatorKey;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.AuthenticatorRegistration.RegisterAuthenticator;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Me;

[TestFixture]
internal class RegisterAuthenticatorTests : TestBase
{
    private CreateUserAccountCommand? _currentUserAccountCommand;
    private CreateUserAccountCommand? _secondUserAccountCommand;

    [Test]
    public async Task RegisterAuthenticator_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var registerAuthenticatorCommand = new RegisterAuthenticatorCommand(userId, "SomeCode");

        // Act
        var registerAuthenticatorResult = await UserAccessModule.ExecuteCommandAsync(registerAuthenticatorCommand);

        // Assert
        Assert.That(registerAuthenticatorResult.IsSuccess, Is.False);
        Assert.That(registerAuthenticatorResult.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task RegisterAuthenticator_ReturnsOk_WhenDataIsValid()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var getAuthenticatorKeyQuery = new GetAuthenticatorKeyQuery(userId);
        var getAuthenticatorKeyResult = await UserAccessModule.ExecuteQueryAsync(getAuthenticatorKeyQuery);
        var authenticator = new Authenticator();
        var code = authenticator.GenerateAuthenticatorCode(getAuthenticatorKeyResult.Value!);
        var registerAuthenticatorCommand = new RegisterAuthenticatorCommand(userId, code);

        // Act
        var registerAuthenticatorResult = await UserAccessModule.ExecuteCommandAsync(registerAuthenticatorCommand);

        // Assert
        Assert.That(registerAuthenticatorResult.IsSuccess, Is.True);
        Assert.That(registerAuthenticatorResult.Status, Is.EqualTo(ResultStatus.Ok));
    }

    [Test]
    public async Task RegisterAuthenticator_ReturnsForbidden_WhenUserIdDoesNotMatchCurrentUser()
    {
        // Arrange
        var userId = _secondUserAccountCommand!.UserId;
        var registerAuthenticatorCommand = new RegisterAuthenticatorCommand(userId, "SomeCode");

        // Act
        var registerAuthenticatorResult = await UserAccessModule.ExecuteCommandAsync(registerAuthenticatorCommand);

        // Assert
        Assert.That(registerAuthenticatorResult.IsSuccess, Is.False);
        Assert.That(registerAuthenticatorResult.Status, Is.EqualTo(ResultStatus.Forbidden));
        Assert.That(registerAuthenticatorResult.Errors, Is.Not.Empty);
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _currentUserAccountCommand = UserAccountGenerator(userId: CurrentUserId).Generate();
        _secondUserAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_currentUserAccountCommand);
        await UserAccessModule.ExecuteCommandAsync(_secondUserAccountCommand);
    }
}