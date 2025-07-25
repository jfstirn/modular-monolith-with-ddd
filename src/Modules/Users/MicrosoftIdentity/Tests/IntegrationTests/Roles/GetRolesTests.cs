using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.GetRoles.Directory;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Roles;

[TestFixture]
internal class GetRolesTests : TestBase
{
    [Test]
    public async Task GetRoles_ReturnsOk_WhenRoleExists()
    {
        // Arrange
        await UserAccessModule.ExecuteCommandAsync(RoleGenerator().Generate());
        await UserAccessModule.ExecuteCommandAsync(RoleGenerator().Generate());
        await UserAccessModule.ExecuteCommandAsync(RoleGenerator().Generate());
        int expectedRoleCount = 3;
        var getRolesQuery = new GetRolesQuery();

        // Act
        var getRolesResult = await UserAccessModule.ExecuteQueryAsync(getRolesQuery);

        // Assert
        Assert.That(getRolesResult.IsSuccess, Is.True);
        Assert.That(getRolesResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(getRolesResult.Value!.Count, Is.EqualTo(expectedRoleCount));
    }

    [Test]
    public async Task GetRoles_ReturnsOk_WhenNoRoleExists()
    {
        // Arrange
        int expectedRoleCount = 0;
        var getRolesQuery = new GetRolesQuery();

        // Act
        var getRolesResult = await UserAccessModule.ExecuteQueryAsync(getRolesQuery);

        // Assert
        Assert.That(getRolesResult.IsSuccess, Is.True);
        Assert.That(getRolesResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(getRolesResult.Value!.Count, Is.EqualTo(expectedRoleCount));
    }
}