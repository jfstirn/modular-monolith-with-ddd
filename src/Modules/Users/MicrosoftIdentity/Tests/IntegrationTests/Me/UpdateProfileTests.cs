using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.UpdateProfile;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Me;

[TestFixture]
internal class UpdateProfileTests : TestBase
{
    private CreateUserAccountCommand? _currentUserAccountCommand;
    private CreateUserAccountCommand? _secondUserAccountCommand;

    [Test]
    public async Task UpdateProfile_ReturnsError_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var updateProfileCommand = new UpdateProfileCommand(userId, "foo.bar", "Foo Bar", "Foo", "Bar");

        // Act
        var updateProfileResult = await UserAccessModule.ExecuteCommandAsync(updateProfileCommand);

        // Assert
        Assert.That(updateProfileResult.IsSuccess, Is.False);
        Assert.That(updateProfileResult.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task UpdateProfile_ReturnsOk_WhenDataIsValid()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var updateProfileCommand = new UpdateProfileCommand(userId, "foo.bar", "Foo Bar", "Foo", "Bar");

        // Act
        var updateProfileResult = await UserAccessModule.ExecuteCommandAsync(updateProfileCommand);

        // Assert
        Assert.That(updateProfileResult.IsSuccess, Is.True);
        Assert.That(updateProfileResult.Status, Is.EqualTo(ResultStatus.Ok));
    }

    [Test]
    public async Task UpdateProfile_ReturnsError_WhenUserNameIsNotUnique()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var userName = _secondUserAccountCommand!.Login;
        var updateProfileCommand = new UpdateProfileCommand(userId, userName, "Foo Bar", "Foo", "Bar");

        // Act
        var updateProfileResult = await UserAccessModule.ExecuteCommandAsync(updateProfileCommand);

        // Assert
        Assert.That(updateProfileResult.IsSuccess, Is.False);
        Assert.That(updateProfileResult.Status, Is.EqualTo(ResultStatus.Error));
    }

    [Test]
    public async Task UpdateProfile_ReturnsForbidden_WhenUserIdDoesNotMatchCurrentUser()
    {
        // Arrange
        var userId = _secondUserAccountCommand!.UserId;
        var updateProfileCommand = new UpdateProfileCommand(userId, "foo.bar", "Foo Bar", "Foo", "Bar");

        // Act
        var updateProfileResult = await UserAccessModule.ExecuteCommandAsync(updateProfileCommand);

        // Assert
        Assert.That(updateProfileResult.IsSuccess, Is.False);
        Assert.That(updateProfileResult.Status, Is.EqualTo(ResultStatus.Forbidden));
        Assert.That(updateProfileResult.Errors, Is.Not.Empty);
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _currentUserAccountCommand = UserAccountGenerator(userId: CurrentUserId).Generate();
        _secondUserAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_currentUserAccountCommand);
        await UserAccessModule.ExecuteCommandAsync(_secondUserAccountCommand);
    }
}