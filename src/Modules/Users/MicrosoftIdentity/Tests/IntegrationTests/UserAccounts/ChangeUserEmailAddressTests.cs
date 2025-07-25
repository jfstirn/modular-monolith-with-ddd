using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.ChangeEmailAddress;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.GetUserAccounts.ById;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.UserAccounts;

[TestFixture]
internal class ChangeUserEmailAddressTests : TestBase
{
    private CreateUserAccountCommand? _userAccountCommand;

    [Test]
    public async Task ChangeUserEmailAddress_ReturnsOk_WhenEmailIsValid()
    {
        // Arrange
        var userId = _userAccountCommand!.UserId;
        var newEmailAddress = "kamil@mymeetings.com";
        var changeEmailCommand = new ChangeUserEmailAddressCommand(userId, newEmailAddress);
        var getUserQuery = new GetUserAccountsQuery(userId);

        // Act
        var changeEmailResult = await UserAccessModule.ExecuteCommandAsync(changeEmailCommand);
        var getUserResult = await UserAccessModule.ExecuteQueryAsync(getUserQuery);

        // Assert
        Assert.That(changeEmailResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(changeEmailResult.IsSuccess, Is.True);
        Assert.That(changeEmailResult.Errors, Is.Empty);
        Assert.That(getUserResult.Value!.Email, Is.EqualTo(newEmailAddress));
    }

    [Test]
    public async Task ChangeUserEmailAddress_ReturnsError_WhenNewEmailAddressIsInvalid()
    {
        // Arrange
        var userId = _userAccountCommand!.UserId;
        var newEmailAddress = "invalid-email";
        var changeEmailCommand = new ChangeUserEmailAddressCommand(userId, newEmailAddress);
        var getUserQuery = new GetUserAccountsQuery(userId);

        // Act
        var changeEmailResult = await UserAccessModule.ExecuteCommandAsync(changeEmailCommand);
        var getUserResult = await UserAccessModule.ExecuteQueryAsync(getUserQuery);

        // Assert
        Assert.That(changeEmailResult.Status, Is.EqualTo(ResultStatus.Error));
        Assert.That(changeEmailResult.IsSuccess, Is.False);
        Assert.That(changeEmailResult.Errors, Is.Not.Empty);
        Assert.That(getUserResult.Value!.Email, Is.EqualTo(_userAccountCommand.EmailAddress));
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _userAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_userAccountCommand);
    }
}