using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.UnlockUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.UserAccounts;

[TestFixture]
internal class UnlockUserAccountTests : TestBase
{
    private CreateUserAccountCommand _userAccountCommand = null!;

    [Test]
    public async Task UnlockUserAccount_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var unlockUserAccountCommand = new UnlockUserAccountCommand(userId);

        // Act
        var unlockUserAccountResult = await UserAccessModule.ExecuteCommandAsync(unlockUserAccountCommand);

        // Assert
        Assert.That(unlockUserAccountResult.IsSuccess, Is.False);
        Assert.That(unlockUserAccountResult.Status, Is.EqualTo(ResultStatus.NotFound));
        Assert.That(unlockUserAccountResult.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task UnlockUserAccount_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var userId = _userAccountCommand.UserId;
        var unlockUserAccountCommand = new UnlockUserAccountCommand(userId);

        // Act
        var unlockUserAccountResult = await UserAccessModule.ExecuteCommandAsync(unlockUserAccountCommand);

        // Assert
        Assert.That(unlockUserAccountResult.IsSuccess, Is.True);
        Assert.That(unlockUserAccountResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(unlockUserAccountResult.Errors, Is.Empty);
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _userAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_userAccountCommand);
    }
}