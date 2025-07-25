using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.SetUserRoles;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.UserAccounts;

[TestFixture]
internal class SetUserRolesTests : TestBase
{
    private CreateUserAccountCommand _userAccountCommand = null!;
    private Guid _roleId;

    [Test]
    public async Task SetUserRoles_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var setUserRolesCommand = new SetUserRolesCommand(userId, [_roleId]);

        // Act
        var setUserRolesResult = await UserAccessModule.ExecuteCommandAsync(setUserRolesCommand);

        // Assert
        Assert.That(setUserRolesResult.IsSuccess, Is.False);
        Assert.That(setUserRolesResult.Status, Is.EqualTo(ResultStatus.NotFound));
        Assert.That(setUserRolesResult.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task SetUserRoles_ReturnsOk_WhenRolesAreValid()
    {
        // Arrange
        var userId = _userAccountCommand.UserId;
        var setUserRolesCommand = new SetUserRolesCommand(userId, [_roleId]);

        // Act
        var setUserRolesResult = await UserAccessModule.ExecuteCommandAsync(setUserRolesCommand);

        // Assert
        Assert.That(setUserRolesResult.IsSuccess, Is.True);
        Assert.That(setUserRolesResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(setUserRolesResult.Errors, Is.Empty);
    }

    [Test]
    public async Task SetUserRoles_ReturnsOk_WhenRolesAreEmpty()
    {
        // Arrange
        var userId = _userAccountCommand.UserId;
        var setUserRolesCommand = new SetUserRolesCommand(userId, []);

        // Act
        var setUserRolesResult = await UserAccessModule.ExecuteCommandAsync(setUserRolesCommand);

        // Assert
        Assert.That(setUserRolesResult.IsSuccess, Is.True);
        Assert.That(setUserRolesResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(setUserRolesResult.Errors, Is.Empty);
    }

    [Test]
    public async Task SetUserRoles_ReturnsNotFound_WhenRoleDoesNotExist()
    {
        // Arrange
        var userId = _userAccountCommand.UserId;
        var setUserRolesCommand = new SetUserRolesCommand(userId, [Guid.NewGuid()]);

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
        var roleCommand = RoleGenerator().Generate();

        await UserAccessModule.ExecuteCommandAsync(_userAccountCommand);
        _roleId = (await UserAccessModule.ExecuteCommandAsync(roleCommand)).Value;
    }
}
