using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.CreateRole;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.RenameRole;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Roles;

[TestFixture]
internal class RenameRoleTests : TestBase
{
    private Guid _roleId;
    private CreateRoleCommand _createRoleCommand = null!;

    [Test]
    public async Task RenameRole_ReturnsNotFound_WhenRoleDoesNotExist()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var newRoleName = "NewRoleName";
        var renameRoleCommand = new RenameRoleCommand(roleId, newRoleName);

        // Act
        var renameRoleResult = await UserAccessModule.ExecuteCommandAsync(renameRoleCommand);

        // Assert
        Assert.That(renameRoleResult.IsSuccess, Is.False);
        Assert.That(renameRoleResult.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task RenameRole_ReturnsError_WhenRoleNameIsInvalid()
    {
        // Arrange
        var invalidRoleName = string.Empty; // Empty role name
        var renameRoleCommand = new RenameRoleCommand(_roleId, invalidRoleName);

        // Act
        var renameRoleResult = await UserAccessModule.ExecuteCommandAsync(renameRoleCommand);

        // Assert
        Assert.That(renameRoleResult.IsSuccess, Is.False);
        Assert.That(renameRoleResult.Status, Is.EqualTo(ResultStatus.Error));
    }

    [Test]
    public async Task RenameRole_ReturnsError_WhenRoleNameAlreadyExists()
    {
        // Arrange
        var anotherRoleCommand = RoleGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(anotherRoleCommand);
        var renameRoleCommand = new RenameRoleCommand(_roleId, anotherRoleCommand.Name);

        // Act
        var renameRoleResult = await UserAccessModule.ExecuteCommandAsync(renameRoleCommand);

        // Assert
        Assert.That(renameRoleResult.IsSuccess, Is.False);
        Assert.That(renameRoleResult.Status, Is.EqualTo(ResultStatus.Error));
    }

    [Test]
    public async Task RenameRole_ReturnsOk_WhenRoleNameIsValid()
    {
        // Arrange
        var newRoleName = "UpdatedRoleName";
        var renameRoleCommand = new RenameRoleCommand(_roleId, newRoleName);

        // Act
        var renameRoleResult = await UserAccessModule.ExecuteCommandAsync(renameRoleCommand);

        // Assert
        Assert.That(renameRoleResult.IsSuccess, Is.True);
        Assert.That(renameRoleResult.Status, Is.EqualTo(ResultStatus.Ok));
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _createRoleCommand = RoleGenerator().Generate();
        var result = await UserAccessModule.ExecuteCommandAsync(_createRoleCommand);
        _roleId = result.Value;
    }
}