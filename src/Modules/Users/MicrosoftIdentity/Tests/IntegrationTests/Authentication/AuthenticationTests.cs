using System.Data;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Authentication.Login;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts.Results;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.Authentication;

[TestFixture]
internal class AuthenticationTests : TestBase
{
    private const string UserAccountPassword = "Password1!";
    private CreateUserAccountCommand _userAccountCommand = null!;

    [Test]
    public async Task Authenticate_ReturnsOk_WhenCredentialsAreValid()
    {
        // Arrange
        var authenticateCommand = new AccountLoginCommand(_userAccountCommand.Login, UserAccountPassword);

        // Act
        var authenticateResult = await UserAccessModule.ExecuteCommandAsync(authenticateCommand);

        // Assert
        Assert.That(authenticateResult.IsSuccess, Is.True);
        Assert.That(authenticateResult.Status, Is.EqualTo(ResultStatus.Ok));
        Assert.That(authenticateResult.AccessToken, Is.Not.Empty);
        Assert.That(authenticateResult.IsAuthenticated, Is.True);
    }

    protected override async Task SeedDatabase(IDbConnection connection)
    {
        _userAccountCommand = UserAccountGenerator(password: UserAccountPassword).Generate();

        await UserAccessModule.ExecuteCommandAsync(_userAccountCommand);
    }
}
