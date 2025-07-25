using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.CreateRole;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.GetUserRoles;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.SetUserRoles;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.UserAccounts;

[TestFixture]
internal class GetUserRolesTests : TestBase
{
    private CreateUserAccountCommand _userAccountWithRolesCommand = null!;
    private CreateUserAccountCommand _userAccountWithoutRolesCommand = null!;
    private CreateRoleCommand _roleCommand = null!;

    [Test]
    public async Task Get_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var getUserRolesQuery = new GetUserRolesQuery(userId);

        // Act
        var getUserRolesResult = await UserAccessModule.ExecuteQueryAsync(getUserRolesQuery);

        // Assert
        Assert.That(getUserRolesResult.Status, Is.EqualTo(ResultStatus.NotFound));
        Assert.That(getUserRolesResult.Value, Is.Null);
    }

    [Test]
    public async Task Get_ReturnsUserRoles_WhenUserExists()
    {
        // Arrange
        var userId = _userAccountWithRolesCommand.UserId;
        var getUserRolesQuery = new GetUserRolesQuery(userId);

        // Act
        var getUserRolesResult = await UserAccessModule.ExecuteQueryAsync(getUserRolesQuery);

        // Assert
        Assert.That(getUserRolesResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(getUserRolesResult.Value!.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task Get_ReturnsEmptyUserRoles_WhenUserHasNoRolesAssigned()
    {
        // Arrange
        var userId = _userAccountWithoutRolesCommand.UserId;
        var getUserRolesQuery = new GetUserRolesQuery(userId);

        // Act
        var getUserRolesResult = await UserAccessModule.ExecuteQueryAsync(getUserRolesQuery);

        // Assert
        Assert.That(getUserRolesResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(getUserRolesResult.Value!.Count, Is.EqualTo(0));
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _userAccountWithRolesCommand = UserAccountGenerator().Generate();
        _userAccountWithoutRolesCommand = UserAccountGenerator().Generate();
        _roleCommand = RoleGenerator().Generate();

        await UserAccessModule.ExecuteCommandAsync(_userAccountWithRolesCommand);
        await UserAccessModule.ExecuteCommandAsync(_userAccountWithoutRolesCommand);
        var roleId = (await UserAccessModule.ExecuteCommandAsync(_roleCommand)).Value;

        // Assign role to user
        var assignRoleCommand = new SetUserRolesCommand(_userAccountWithRolesCommand.UserId, [roleId]);
        await UserAccessModule.ExecuteCommandAsync(assignRoleCommand);
    }
}
