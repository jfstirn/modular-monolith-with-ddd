using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.RequestChangeEmailAddressToken;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Me;

[TestFixture]
internal class RequestChangeEmailAddressTokenTests : TestBase
{
    private CreateUserAccountCommand? _currentUserAccountCommand;
    private CreateUserAccountCommand? _secondUserAccountCommand;

    [Test]
    public async Task RequestChangeEmailAddressToken_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var newEmailAddress = "hallo@mymeetings.com";
        var requestChangeEmailAddressTokenCommand = new RequestChangeEmailAddressTokenCommand(userId, newEmailAddress);

        // Act
        var requestChangeEmailAddressTokenResult = await UserAccessModule.ExecuteCommandAsync(requestChangeEmailAddressTokenCommand);

        // Assert
        Assert.That(requestChangeEmailAddressTokenResult.IsSuccess, Is.False);
        Assert.That(requestChangeEmailAddressTokenResult.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task RequestChangeEmailAddressToken_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var newEmailAddress = "hallo@mymeetings.com";
        var requestChangeEmailAddressTokenCommand = new RequestChangeEmailAddressTokenCommand(userId, newEmailAddress);

        // Act
        var requestChangeEmailAddressTokenResult = await UserAccessModule.ExecuteCommandAsync(requestChangeEmailAddressTokenCommand);

        // Assert
        Assert.That(requestChangeEmailAddressTokenResult.IsSuccess, Is.True);
        Assert.That(requestChangeEmailAddressTokenResult.Status, Is.EqualTo(ResultStatus.Ok));
    }

    [Test]
    public async Task RequestChangeEmailAddressToken_SendsEmail_WhenUserExists()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var newEmailAddress = "hallo@mymeetings.com";
        var requestChangeEmailAddressTokenCommand = new RequestChangeEmailAddressTokenCommand(userId, newEmailAddress);

        // Act
        var requestChangeEmailAddressTokenResult = await UserAccessModule.ExecuteCommandAsync(requestChangeEmailAddressTokenCommand);

        // Assert
        Assert.That(EmailSender.EmailMessage, Is.Not.Null);
    }

    [Test]
    public async Task RequestChangeEmailAddressToken_ReturnsForbidden_WhenUserIdDoesNotMatchCurrentUser()
    {
        // Arrange
        var userId = _secondUserAccountCommand!.UserId;
        var newEmailAddress = "hallo@mymeetings.com";
        var requestChangeEmailAddressTokenCommand = new RequestChangeEmailAddressTokenCommand(userId, newEmailAddress);

        // Act
        var requestChangeEmailAddressTokenResult = await UserAccessModule.ExecuteCommandAsync(requestChangeEmailAddressTokenCommand);

        // Assert
        Assert.That(requestChangeEmailAddressTokenResult.IsSuccess, Is.False);
        Assert.That(requestChangeEmailAddressTokenResult.Status, Is.EqualTo(ResultStatus.Forbidden));
        Assert.That(requestChangeEmailAddressTokenResult.Errors, Is.Not.Empty);
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _currentUserAccountCommand = UserAccountGenerator(userId: CurrentUserId).Generate();
        _secondUserAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_currentUserAccountCommand);
        await UserAccessModule.ExecuteCommandAsync(_secondUserAccountCommand);
    }
}