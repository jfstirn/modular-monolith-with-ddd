using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.CreateRole;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRolePermissions;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Roles;

[TestFixture]
internal class GetRolePermissionsTests : TestBase
{
    private Guid _roleId;
    private CreateRoleCommand _createRoleCommand = null!;

    [Test]
    public async Task GetRolePermissions_ReturnsNotFound_WhenRoleDoesNotExist()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var getRolePermissionsQuery = new GetRolePermissionsQuery(roleId);

        // Act
        var getRolePermissionsResult = await UserAccessModule.ExecuteQueryAsync(getRolePermissionsQuery);

        // Assert
        Assert.That(getRolePermissionsResult.IsSuccess, Is.False);
        Assert.That(getRolePermissionsResult.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task GetRolePermissions_ReturnsOk_WhenRoleExists()
    {
        // Arrange
        var getRolePermissionsQuery = new GetRolePermissionsQuery(_roleId);

        // Act
        var getRolePermissionsResult = await UserAccessModule.ExecuteQueryAsync(getRolePermissionsQuery);
        var permissions = getRolePermissionsResult.Value?.Select(x => x.Code) ?? Enumerable.Empty<string>();

        // Assert
        Assert.That(getRolePermissionsResult.IsSuccess, Is.True);
        Assert.That(getRolePermissionsResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(_createRoleCommand.Permissions, Is.EquivalentTo(permissions));
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        await ExecuteScript("Roles/0002_SeedPermissions.sql");

        _createRoleCommand = RoleGenerator(permissions: ["Users.CreateUserAccount", "Users.AddRole"]).Generate();
        var result = await UserAccessModule.ExecuteCommandAsync(_createRoleCommand);
        _roleId = result.Value;
    }
}