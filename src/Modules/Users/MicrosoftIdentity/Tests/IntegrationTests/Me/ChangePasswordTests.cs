using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Me.ChangePassword;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Me;

[TestFixture]
internal class ChangePasswordTests : TestBase
{
    private const string UserAccountPassword = "Password1!";
    private CreateUserAccountCommand? _currentUserAccountCommand;
    private CreateUserAccountCommand? _secondUserAccountCommand;

    [Test]
    public async Task ChangePassword_ReturnsError_WhenCurrentPasswordIsInvalid()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var changePasswordCommand = new ChangePasswordCommand(
            userId,
            "InvalidCurrentPassword1!",
            "NewPassword1!");

        // Act
        var changePasswordResult = await UserAccessModule.ExecuteCommandAsync(changePasswordCommand);

        // Assert
        Assert.That(changePasswordResult.Status, Is.EqualTo(ResultStatus.Error));
        Assert.That(changePasswordResult.IsSuccess, Is.False);
        Assert.That(changePasswordResult.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task ChangePassword_ReturnsOk_WhenCurrentPasswordIsValid()
    {
        // Arrange
        var userId = _currentUserAccountCommand!.UserId;
        var changePasswordCommand = new ChangePasswordCommand(
            userId,
            UserAccountPassword,
            "NewPassword1!");

        // Act
        var changePasswordResult = await UserAccessModule.ExecuteCommandAsync(changePasswordCommand);

        // Assert
        Assert.That(changePasswordResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(changePasswordResult.IsSuccess, Is.True);
        Assert.That(changePasswordResult.Errors, Is.Empty);
    }

    [Test]
    public async Task ChangePassword_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var changePasswordCommand = new ChangePasswordCommand(
            userId,
            "SomeCurrentPassword1!",
            "NewPassword1!");

        // Act
        var changePasswordResult = await UserAccessModule.ExecuteCommandAsync(changePasswordCommand);

        // Assert
        Assert.That(changePasswordResult.Status, Is.EqualTo(ResultStatus.NotFound));
        Assert.That(changePasswordResult.IsSuccess, Is.False);
        Assert.That(changePasswordResult.Errors, Is.Not.Empty);
    }

    [Test]
    public async Task ChangePassword_ReturnsForbidden_WhenUserIdDoesNotMatchCurrentUser()
    {
        // Arrange
        var userId = _secondUserAccountCommand!.UserId;
        var changePasswordCommand = new ChangePasswordCommand(
            userId,
            UserAccountPassword,
            "NewPassword1!");

        // Act
        var changePasswordResult = await UserAccessModule.ExecuteCommandAsync(changePasswordCommand);

        // Assert
        Assert.That(changePasswordResult.Status, Is.EqualTo(ResultStatus.Forbidden));
        Assert.That(changePasswordResult.IsSuccess, Is.False);
        Assert.That(changePasswordResult.Errors, Is.Not.Empty);
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _currentUserAccountCommand = UserAccountGenerator(userId: CurrentUserId, password: UserAccountPassword).Generate();
        _secondUserAccountCommand = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(_currentUserAccountCommand);
        await UserAccessModule.ExecuteCommandAsync(_secondUserAccountCommand);
    }
}