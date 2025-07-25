using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.SetUserPermissions;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.UserAccounts;

[TestFixture]
internal class SetUserPermissionsTests : TestBase
{
    private CreateUserAccountCommand _userAccountCommand = null!;

    [Test]
    public async Task SetUserPermissions_ReturnsOk_WhenPermissionsAreValid()
    {
        // Arrange
        var userId = _userAccountCommand.UserId;
        var setUserRolesCommand = new SetUserPermissionsCommand(userId, ["Users.CreateUserAccount", "Users.AddRole"]);

        // Act
        var setUserRolesResult = await UserAccessModule.ExecuteCommandAsync(setUserRolesCommand);

        // Assert
        Assert.That(setUserRolesResult.IsSuccess, Is.True);
        Assert.That(setUserRolesResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(setUserRolesResult.Errors, Is.Empty);
    }

    [Test]
    public async Task SetUserPermissions_ReturnsOk_WhenPermissionsAreEmpty()
    {
        // Arrange
        var userId = _userAccountCommand.UserId;
        var setUserRolesCommand = new SetUserPermissionsCommand(userId, []);

        // Act
        var setUserRolesResult = await UserAccessModule.ExecuteCommandAsync(setUserRolesCommand);

        // Assert
        Assert.That(setUserRolesResult.IsSuccess, Is.True);
        Assert.That(setUserRolesResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(setUserRolesResult.Errors, Is.Empty);
    }

    [Test]
    public async Task SetUserPermissions_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var setUserRolesCommand = new SetUserPermissionsCommand(userId, []);

        // Act
        var setUserRolesResult = await UserAccessModule.ExecuteCommandAsync(setUserRolesCommand);

        // Assert
        Assert.That(setUserRolesResult.IsSuccess, Is.False);
        Assert.That(setUserRolesResult.Status, Is.EqualTo(ResultStatus.NotFound));
        Assert.That(setUserRolesResult.Errors, Is.Not.Empty);
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _userAccountCommand = UserAccountGenerator().Generate();

        await UserAccessModule.ExecuteCommandAsync(_userAccountCommand);
    }
}
