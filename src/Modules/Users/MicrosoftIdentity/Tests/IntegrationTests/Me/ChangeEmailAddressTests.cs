using System.Data;
using System.Text.RegularExpressions;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.ChangeEmailAddress;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.RequestChangeEmailAddressToken;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Me;

[TestFixture]
internal class ChangeEmailAddressTests : TestBase
{
    private CreateUserAccountCommand? _currentUserAccountCommand;
    private CreateUserAccountCommand? _secondUserAccountCommand;

    [Test]
    public async Task ChangeEmailAddress_ReturnsOk_WhenTokenIsValid()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var newEmailAddress = "hallo@mymeetings.com";

        // To change an email address, the user must first request a token to change the email address.
        // The token gets sent by email, so we have to parse the content of the email message to get it.
        var requestChangeEmailAddressTokenCommand = new RequestChangeEmailAddressTokenCommand(userId, newEmailAddress);
        await UserAccessModule.ExecuteCommandAsync(requestChangeEmailAddressTokenCommand);
        string pattern = @":\s*([A-Za-z0-9\/\+=]+)";
        var match = Regex.Matches(EmailSender.EmailMessage?.Content ?? string.Empty, pattern, RegexOptions.Multiline)
                         .Cast<Match>()
                         .LastOrDefault();
        string token = match?.Groups[1].Value ?? string.Empty;

        var changeEmailAddressCommand = new ChangeEmailAddressCommand(userId, newEmailAddress, token);

        // Act
        var changeEmailAddressResult = await UserAccessModule.ExecuteCommandAsync(changeEmailAddressCommand);

        // Assert
        Assert.That(changeEmailAddressResult.IsSuccess, Is.True);
        Assert.That(changeEmailAddressResult.Status, Is.EqualTo(ResultStatus.Ok));
    }

    [Test]
    public async Task ChangeEmailAddress_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var newEmailAddress = "hallo@mymeetings.com";
        var changeEmailAddressCommand = new ChangeEmailAddressCommand(userId, newEmailAddress, "some-token");

        // Act
        var changeEmailAddressResult = await UserAccessModule.ExecuteCommandAsync(changeEmailAddressCommand);

        // Assert
        Assert.That(changeEmailAddressResult.IsSuccess, Is.False);
        Assert.That(changeEmailAddressResult.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task ChangeEmailAddress_ReturnsError_WhenEmailAddressIsInvalid()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var newEmailAddress = "invalid-email";
        var changeEmailAddressCommand = new ChangeEmailAddressCommand(userId, newEmailAddress, "some-token");

        // Act
        var changeEmailAddressResult = await UserAccessModule.ExecuteCommandAsync(changeEmailAddressCommand);

        // Assert
        Assert.That(changeEmailAddressResult.IsSuccess, Is.False);
        Assert.That(changeEmailAddressResult.Status, Is.EqualTo(ResultStatus.Error));
    }

    [Test]
    public async Task ChangeEmailAddress_ReturnsForbidden_WhenUserIdDoesNotMatchCurrentUser()
    {
        // Arrange
        var userId = _secondUserAccountCommand!.UserId;
        var newEmailAddress = "hallo@mymeetings.com";
        var changeEmailAddressCommand = new ChangeEmailAddressCommand(userId, newEmailAddress, "some-token");

        // Act
        var changeEmailAddressResult = await UserAccessModule.ExecuteCommandAsync(changeEmailAddressCommand);

        // Assert
        Assert.That(changeEmailAddressResult.Status, Is.EqualTo(ResultStatus.Forbidden));
        Assert.That(changeEmailAddressResult.IsSuccess, Is.False);
        Assert.That(changeEmailAddressResult.Errors, Is.Not.Empty);
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _currentUserAccountCommand = UserAccountGenerator(userId: CurrentUserId).Generate();
        _secondUserAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_currentUserAccountCommand);
        await UserAccessModule.ExecuteCommandAsync(_secondUserAccountCommand);
    }
}