using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.RequestConfirmEmailAddressToken;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Me;

[TestFixture]
internal class RequestConfirmEmailAddressTokenTests : TestBase
{
    private CreateUserAccountCommand? _currentUserAccountCommand;
    private CreateUserAccountCommand? _secondUserAccountCommand;

    [Test]
    public async Task RequestConfirmEmailAddressToken_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var requestConfirmEmailAddressTokenCommand = new RequestConfirmEmailAddressTokenCommand(userId);

        // Act
        var requestConfirmEmailAddressTokenResult = await UserAccessModule.ExecuteCommandAsync(requestConfirmEmailAddressTokenCommand);

        // Assert
        Assert.That(requestConfirmEmailAddressTokenResult.IsSuccess, Is.False);
        Assert.That(requestConfirmEmailAddressTokenResult.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task RequestConfirmEmailAddressToken_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var requestConfirmEmailAddressTokenCommand = new RequestConfirmEmailAddressTokenCommand(userId);

        // Act
        var requestConfirmEmailAddressTokenResult = await UserAccessModule.ExecuteCommandAsync(requestConfirmEmailAddressTokenCommand);

        // Assert
        Assert.That(requestConfirmEmailAddressTokenResult.IsSuccess, Is.True);
        Assert.That(requestConfirmEmailAddressTokenResult.Status, Is.EqualTo(ResultStatus.Ok));
    }

    [Test]
    public async Task RequestConfirmEmailAddressToken_SendsEmail_WhenUserExists()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var requestConfirmEmailAddressTokenCommand = new RequestConfirmEmailAddressTokenCommand(userId);

        // Act
        var requestConfirmEmailAddressTokenResult = await UserAccessModule.ExecuteCommandAsync(requestConfirmEmailAddressTokenCommand);

        // Assert
        Assert.That(EmailSender.EmailMessage, Is.Not.Null);
    }

    [Test]
    public async Task RequestConfirmEmailAddressToken_ReturnsForbidden_WhenUserIdDoesNotMatchCurrentUser()
    {
        // Arrange
        var userId = _secondUserAccountCommand!.UserId;
        var requestConfirmEmailAddressTokenCommand = new RequestConfirmEmailAddressTokenCommand(userId);

        // Act
        var requestConfirmEmailAddressTokenResult = await UserAccessModule.ExecuteCommandAsync(requestConfirmEmailAddressTokenCommand);

        // Assert
        Assert.That(requestConfirmEmailAddressTokenResult.IsSuccess, Is.False);
        Assert.That(requestConfirmEmailAddressTokenResult.Status, Is.EqualTo(ResultStatus.Forbidden));
        Assert.That(requestConfirmEmailAddressTokenResult.Errors, Is.Not.Empty);
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _currentUserAccountCommand = UserAccountGenerator(userId: CurrentUserId).Generate();
        _secondUserAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_currentUserAccountCommand);
        await UserAccessModule.ExecuteCommandAsync(_secondUserAccountCommand);
    }
}