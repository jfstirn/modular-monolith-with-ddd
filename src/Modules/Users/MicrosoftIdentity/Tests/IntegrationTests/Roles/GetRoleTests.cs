using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.CreateRole;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRoles.ById;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Roles;

[TestFixture]
internal class GetRoleTests : TestBase
{
    private Guid _roleId;
    private CreateRoleCommand _createRoleCommand = null!;

    [Test]
    public async Task GetRole_ReturnsNotFound_WhenRoleDoesNotExist()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var getRoleQuery = new GetRolesQuery(roleId);

        // Act
        var getRoleResult = await UserAccessModule.ExecuteQueryAsync(getRoleQuery);

        // Assert
        Assert.That(getRoleResult.IsSuccess, Is.False);
        Assert.That(getRoleResult.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task GetRole_ReturnsOk_WhenRoleExists()
    {
        // Arrange
        var getRoleQuery = new GetRolesQuery(_roleId);

        // Act
        var getRoleResult = await UserAccessModule.ExecuteQueryAsync(getRoleQuery);
        var role = getRoleResult.Value;

        // Assert
        Assert.That(getRoleResult.IsSuccess, Is.True);
        Assert.That(getRoleResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(role?.Name, Is.EqualTo(_createRoleCommand.Name));
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _createRoleCommand = RoleGenerator().Generate();
        var result = await UserAccessModule.ExecuteCommandAsync(_createRoleCommand);
        _roleId = result.Value;
    }
}