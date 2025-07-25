using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.UserAccounts;

[TestFixture]
internal class CreateUserTests : TestBase
{
    [Test]
    public async Task Create_ReturnsCreated_WhenUserIsValid()
    {
        // Arrange
        var command = UserAccountGenerator().Generate();

        // Act
        var result = await UserAccessModule.ExecuteCommandAsync(command);

        // Assert
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Created));
    }

    [Test]
    public async Task Create_ReturnsError_WhenNameIsInvalid()
    {
        // Arrange
        const string invalidName = "";
        var request = UserAccountGenerator(login: invalidName)
            .Clone().Generate();

        // Act
        var result = await UserAccessModule.ExecuteCommandAsync(request);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Error));
    }

    [Test]
    public async Task Create_ReturnsOk_WhenUserNameAlreadyExists()
    {
        // Arrange
        var request = UserAccountGenerator().Generate();
        await UserAccessModule.ExecuteCommandAsync(request);

        // Act
        var result = await UserAccessModule.ExecuteCommandAsync(request);

        // Assert
        // As the system is designed to not leak information about existing users,
        // the response should be of status Ok instead of Created. And the user id should be a fictive Guid.
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(result.Value, Is.Not.EqualTo(Guid.Empty));
    }
}