using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Roles;

[TestFixture]
internal class CreateRoleTests : TestBase
{
    [Test]
    public async Task CreateRole_ReturnsOk_WhenRoleNameIsValid()
    {
        // Arrange
        var createRoleCommand = RoleGenerator().Generate();

        // Act
        var createRoleResult = await UserAccessModule.ExecuteCommandAsync(createRoleCommand);

        // Assert
        Assert.That(createRoleResult.IsSuccess, Is.True);
        Assert.That(createRoleResult.Status, Is.EqualTo(ResultStatus.Created));
    }

    [Test]
    public async Task CreateRole_ReturnsOk_WhenRolePermissionsAreEmpty()
    {
        // Arrange
        var createRoleCommand = RoleGenerator(permissions: Enumerable.Empty<string>()).Generate();

        // Act
        var createRoleResult = await UserAccessModule.ExecuteCommandAsync(createRoleCommand);

        // Assert
        Assert.That(createRoleResult.IsSuccess, Is.True);
        Assert.That(createRoleResult.Status, Is.EqualTo(ResultStatus.Created));
    }

    [Test]
    public async Task CreateRole_ReturnsOk_WhenRolePermissionsAreValid()
    {
        // Arrange
        var createRoleCommand = RoleGenerator(permissions: ["Users.CreateUserAccount", "Users.AddRole"]).Generate();

        // Act
        var createRoleResult = await UserAccessModule.ExecuteCommandAsync(createRoleCommand);

        // Assert
        Assert.That(createRoleResult.IsSuccess, Is.True);
        Assert.That(createRoleResult.Status, Is.EqualTo(ResultStatus.Created));
    }

    [Test]
    public async Task CreateRole_ReturnsError_WhenRoleNameAlreadyExists()
    {
        // Arrange
        var createRoleCommand = RoleGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(createRoleCommand);

        // Act
        var createRoleResult = await UserAccessModule.ExecuteCommandAsync(createRoleCommand);

        // Assert
        Assert.That(createRoleResult.IsSuccess, Is.False);
        Assert.That(createRoleResult.Status, Is.EqualTo(ResultStatus.Error));
    }

    [Test]
    public async Task CreateRole_ReturnsError_WhenRoleNameIsEmpty()
    {
        // Arrange
        var createRoleCommand = RoleGenerator(roleName: string.Empty).Generate();

        // Act
        var createRoleResult = await UserAccessModule.ExecuteCommandAsync(createRoleCommand);

        // Assert
        Assert.That(createRoleResult.IsSuccess, Is.False);
        Assert.That(createRoleResult.Status, Is.EqualTo(ResultStatus.Error));
    }
}