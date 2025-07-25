using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.UpdateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.UserAccounts;

[TestFixture]
internal class UpdateUserAccountTests : TestBase
{
    private CreateUserAccountCommand _userAccountCommand = null!;

    [Test]
    public async Task UpdateUserAccount_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var updateUserAccountCommand = new UpdateUserAccountCommand(userId, string.Empty, string.Empty, string.Empty);

        // Act
        var updateUserAccountResult = await UserAccessModule.ExecuteCommandAsync(updateUserAccountCommand);

        // Assert
        Assert.That(updateUserAccountResult.IsSuccess, Is.False);
        Assert.That(updateUserAccountResult.Status, Is.EqualTo(ResultStatus.NotFound));
        Assert.That(updateUserAccountResult.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task UpdateUserAccount_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var userId = _userAccountCommand.UserId;
        var updateUserAccountCommand = new UpdateUserAccountCommand(userId, "newName", "newFirstName", "newLastName");

        // Act
        var updateUserAccountResult = await UserAccessModule.ExecuteCommandAsync(updateUserAccountCommand);

        // Assert
        Assert.That(updateUserAccountResult.IsSuccess, Is.True);
        Assert.That(updateUserAccountResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(updateUserAccountResult.Errors, Is.Empty);
    }

    [Test]
    public async Task UpdateUserAccount_ReturnsError_WhenDataIsInvalid()
    {
        // Arrange
        var userId = _userAccountCommand.UserId;
        var invalidName = string.Empty;
        var updateUserAccountCommand = new UpdateUserAccountCommand(userId, invalidName, invalidName, invalidName);

        // Act
        var updateUserAccountResult = await UserAccessModule.ExecuteCommandAsync(updateUserAccountCommand);

        // Assert
        Assert.That(updateUserAccountResult.IsSuccess, Is.False);
        Assert.That(updateUserAccountResult.Status, Is.EqualTo(ResultStatus.Error));
        Assert.That(updateUserAccountResult.Errors, Is.Not.Empty);
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _userAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_userAccountCommand);
    }
}