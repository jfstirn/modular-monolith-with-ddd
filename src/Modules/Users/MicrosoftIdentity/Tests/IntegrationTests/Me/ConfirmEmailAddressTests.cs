using System.Data;
using System.Text.RegularExpressions;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.ConfirmEmailAddress;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.RequestConfirmEmailAddressToken;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Me;

[TestFixture]
internal class ConfirmEmailAddressTests : TestBase
{
    private CreateUserAccountCommand? _currentUserAccountCommand;
    private CreateUserAccountCommand? _secondUserAccountCommand;

    [Test]
    public async Task ConfirmEmailAddress_ReturnsOk_WhenTokenIsValid()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;

        // To confirm an email address, we first need to get the confirmation token.
        // The token gets sent by email, so we have to parse the content of the email message to get it.
        var requestEmailConfirmationTokenCommand = new RequestConfirmEmailAddressTokenCommand(userId);
        await UserAccessModule.ExecuteCommandAsync(requestEmailConfirmationTokenCommand);
        string pattern = @":\s*([A-Za-z0-9\/\+=]+)";
        var match = Regex.Matches(EmailSender.EmailMessage?.Content ?? string.Empty, pattern, RegexOptions.Multiline)
                         .Cast<Match>()
                         .LastOrDefault();
        string token = match?.Groups[1].Value ?? string.Empty;

        var confirmEmailAddressCommand = new ConfirmEmailAddressCommand(userId, token);

        // Act
        var confirmEmailAddressResult = await UserAccessModule.ExecuteCommandAsync(confirmEmailAddressCommand);

        // Assert
        Assert.That(confirmEmailAddressResult.IsSuccess, Is.True);
        Assert.That(confirmEmailAddressResult.Status, Is.EqualTo(ResultStatus.Ok));
    }

    [Test]
    public async Task ConfirmEmailAddress_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var confirmEmailAddressCommand = new ConfirmEmailAddressCommand(userId, "some-token");

        // Act
        var confirmEmailAddressResult = await UserAccessModule.ExecuteCommandAsync(confirmEmailAddressCommand);

        // Assert
        Assert.That(confirmEmailAddressResult.IsSuccess, Is.False);
        Assert.That(confirmEmailAddressResult.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task ConfirmEmailAddress_ReturnsError_WhenTokenIsInvalid()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var confirmEmailAddressCommand = new ConfirmEmailAddressCommand(userId, "invalid-token");

        // Act
        var confirmEmailAddressResult = await UserAccessModule.ExecuteCommandAsync(confirmEmailAddressCommand);

        // Assert
        Assert.That(confirmEmailAddressResult.IsSuccess, Is.False);
        Assert.That(confirmEmailAddressResult.Status, Is.EqualTo(ResultStatus.Error));
    }

    [Test]
    public async Task ConfirmEmailAddress_ReturnsForbidden_WhenUserIdDoesNotMatchCurrentUser()
    {
        // Arrange
        var userId = _secondUserAccountCommand!.UserId;
        var confirmEmailAddressCommand = new ConfirmEmailAddressCommand(userId, "some-token");

        // Act
        var confirmEmailAddressResult = await UserAccessModule.ExecuteCommandAsync(confirmEmailAddressCommand);

        // Assert
        Assert.That(confirmEmailAddressResult.IsSuccess, Is.False);
        Assert.That(confirmEmailAddressResult.Status, Is.EqualTo(ResultStatus.Forbidden));
        Assert.That(confirmEmailAddressResult.Errors, Is.Not.Empty);
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _currentUserAccountCommand = UserAccountGenerator(userId: CurrentUserId).Generate();
        _secondUserAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_currentUserAccountCommand);
        await UserAccessModule.ExecuteCommandAsync(_secondUserAccountCommand);
    }
}