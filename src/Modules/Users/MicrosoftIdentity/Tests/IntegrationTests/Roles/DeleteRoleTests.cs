using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.CreateRole;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.DeleteRole;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Roles;

[TestFixture]
internal class DeleteRoleTests : TestBase
{
    private Guid _roleId;
    private CreateRoleCommand _createRoleCommand = null!;

    [Test]
    public async Task DeleteRole_ReturnsOk_WhenRoleExists()
    {
        // Arrange
        var deleteRoleCommand = new DeleteRoleCommand(_roleId);

        // Act
        var deleteRoleResult = await UserAccessModule.ExecuteCommandAsync(deleteRoleCommand);

        // Assert
        Assert.That(deleteRoleResult.IsSuccess, Is.True);
        Assert.That(deleteRoleResult.Status, Is.EqualTo(ResultStatus.Ok));
    }

    [Test]
    public async Task DeleteRole_ReturnsNotFound_WhenRoleDoesNotExist()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var deleteRoleCommand = new DeleteRoleCommand(roleId);

        // Act
        var deleteRoleResult = await UserAccessModule.ExecuteCommandAsync(deleteRoleCommand);

        // Assert
        Assert.That(deleteRoleResult.IsSuccess, Is.False);
        Assert.That(deleteRoleResult.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _createRoleCommand = RoleGenerator().Generate();
        var result = await UserAccessModule.ExecuteCommandAsync(_createRoleCommand);
        _roleId = result.Value;
    }
}
