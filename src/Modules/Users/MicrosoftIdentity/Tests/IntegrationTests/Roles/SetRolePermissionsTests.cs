using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRolePermissions;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.SetRolePermissions;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Roles;

[TestFixture]
internal class SetRolePermissionsTests : TestBase
{
    private Guid _roleId;

    [Test]
    public async Task SetRolePermissions_ReturnsOk_WhenRolePermissionsAreEmpty()
    {
        // Arrange
        var expectedPermissionCount = 0;
        var newPermissions = Array.Empty<string>();
        var setRolePermissionsCommand = new SetRolePermissionsCommand(_roleId, newPermissions);
        var getRolePermissionsQuery = new GetRolePermissionsQuery(_roleId);

        // Act
        var setRolePermissionsResult = await UserAccessModule.ExecuteCommandAsync(setRolePermissionsCommand);
        var getRolePermissionsResult = await UserAccessModule.ExecuteQueryAsync(getRolePermissionsQuery);

        // Assert
        Assert.That(setRolePermissionsResult.IsSuccess, Is.True);
        Assert.That(setRolePermissionsResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(getRolePermissionsResult.IsSuccess, Is.True);
        Assert.That(getRolePermissionsResult.Value!.Count(), Is.EqualTo(expectedPermissionCount));
    }

    [Test]
    public async Task SetRolePermissions_ReturnsNotFound_WhenRoleDoesNotExist()
    {
        // Arrange
        var nonExistentRoleId = Guid.NewGuid();
        var newPermissions = Array.Empty<string>();
        var setRolePermissionsCommand = new SetRolePermissionsCommand(nonExistentRoleId, newPermissions);

        // Act
        var setRolePermissionsResult = await UserAccessModule.ExecuteCommandAsync(setRolePermissionsCommand);

        // Assert
        Assert.That(setRolePermissionsResult.IsSuccess, Is.False);
        Assert.That(setRolePermissionsResult.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task SetRolePermissions_ReturnsOk_WhenPermissionsAreValid()
    {
        // Arrange
        var expectedPermissionCount = 2;
        var newPermissions = new[] { "Users.GetUserRoles", "Users.SetUserRoles" };
        var setRolePermissionsCommand = new SetRolePermissionsCommand(_roleId, newPermissions);
        var getRolePermissionsQuery = new GetRolePermissionsQuery(_roleId);

        // Act
        var setRolePermissionsResult = await UserAccessModule.ExecuteCommandAsync(setRolePermissionsCommand);
        var getRolePermissionsResult = await UserAccessModule.ExecuteQueryAsync(getRolePermissionsQuery);
        var permissions = getRolePermissionsResult.Value?.Select(x => x.Code) ?? Enumerable.Empty<string>();

        // Assert
        Assert.That(setRolePermissionsResult.IsSuccess, Is.True);
        Assert.That(setRolePermissionsResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(getRolePermissionsResult.IsSuccess, Is.True);
        Assert.That(getRolePermissionsResult.Value!.Count(), Is.EqualTo(expectedPermissionCount));
        Assert.That(newPermissions, Is.EquivalentTo(permissions));
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        await ExecuteScript("Roles/0002_SeedPermissions.sql");

        var createRoleCommand = RoleGenerator(permissions: ["Users.GetUsers", "Users.UnlockUserAccount"]).Generate();
        var result = await UserAccessModule.ExecuteCommandAsync(createRoleCommand);
        _roleId = result.Value;
    }
}