using System.Data;
using System.Data.SqlClient;
using Bogus;
using CompanyName.MyMeetings.BuildingBlocks.Application.Security;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Emails;
using CompanyName.MyMeetings.BuildingBlocks.IntegrationTests;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Contracts;
using CompanyName.MyMeetings.Modules.UsersMI.Application.Roles.CreateRole;
using CompanyName.MyMeetings.Modules.UsersMI.Application.UserAccounts.CreateUserAccount;
using CompanyName.MyMeetings.Modules.UsersMI.Infrastructure;
using CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration;
using Dapper;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using Serilog;

namespace CompanyName.MyMeetings.Modules.UsersMI.IntegrationTests.SeedWork
{
    public class TestBase
    {
        [SetUp]
        public async Task BeforeEachTest()
        {
            const string connectionStringEnvironmentVariable =
                "ASPNETCORE_MyMeetings_IntegrationTests_ConnectionString";
            ConnectionString = EnvironmentVariablesProvider.GetVariable(connectionStringEnvironmentVariable);
            if (ConnectionString == null)
            {
                throw new ApplicationException(
                    $"Define connection string to integration tests database using environment variable: {connectionStringEnvironmentVariable}");
            }

            Logger = Substitute.For<ILogger>();
            EmailSender = new EmailSender();
            CurrentUserId = Guid.NewGuid();

            UserAccessStartup.Initialize(
                ConnectionString,
                new ExecutionContextMock(CurrentUserId),
                Logger,
                new EmailsConfiguration("from@email.com"),
                "key",
                EmailSender,
                null,
                new UserAccessConfiguration()
                {
                    Security = new Security()
                    {
                        JwtAudience = "api://mymeetings.com/api",
                        JwtIssuer = "api://mymeetings.com/api",
                        JwtTokenLifetimeInMinutes = 60,
                        JwtSecretKey = "very_long_and_secure_key_that_should_beSoredInA_secure_way"
                    }
                });

            UserAccessModule = new UserAccessModule();

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                await ClearDatabase(sqlConnection);
                await SeedDatabase(sqlConnection);
            }
        }

        protected Guid CurrentUserId { get; private set; }

        protected string ConnectionString { get; private set; } = null!;

        protected ILogger Logger { get; private set; } = null!;

        protected IUserAccessModule UserAccessModule { get; private set; } = null!;

        protected EmailSender EmailSender { get; private set; } = null!;

        protected Faker<CreateUserAccountCommand> UserAccountGenerator(string? login = null, string? password = null, Guid? userId = null) =>
            new Faker<CreateUserAccountCommand>()
            .CustomInstantiator(f =>
            {
                var firstName = f.Name.FirstName();
                var lastName = f.Name.LastName();
                return new CreateUserAccountCommand(
                    id: Guid.NewGuid(),
                    userId: userId ?? Guid.NewGuid(),
                    login: login ?? $"{firstName}.{lastName}".Replace("'", string.Empty).ToLower(),
                    password: PasswordManager.HashPassword(password ?? f.Internet.Password()),
                    name: $"{firstName} {lastName}",
                    firstName: firstName,
                    lastName: lastName,
                    emailAddress: f.Internet.Email());
            });

        protected Faker<CreateRoleCommand> RoleGenerator(string? roleName = null, IEnumerable<string>? permissions = null) =>
            new Faker<CreateRoleCommand>()
            .CustomInstantiator(f =>
            {
                return new CreateRoleCommand(
                    name: roleName ?? f.Lorem.Word().ToLower(),
                    permissions);
            });

        protected async Task<T> GetLastOutboxMessage<T>()
            where T : class, INotification
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var messages = await OutboxMessagesHelper.GetOutboxMessages(connection);

                return OutboxMessagesHelper.Deserialize<T>(messages.Last());
            }
        }

        protected async Task ExecuteScript(string scriptPath)
        {
            var sql = await File.ReadAllTextAsync(scriptPath);

            await using var sqlConnection = new SqlConnection(ConnectionString);
            await sqlConnection.ExecuteScalarAsync(sql);
        }

        protected virtual Task SeedDatabase(IDbConnection connection)
        {
            return Task.CompletedTask;
        }

        private static async Task ClearDatabase(IDbConnection connection)
        {
            const string sql = "DELETE FROM [usersmi].[InboxMessages] " +
                               "DELETE FROM [usersmi].[InternalCommands] " +
                               "DELETE FROM [usersmi].[OutboxMessages] " +
                               "DELETE FROM [usersmi].[UserRoles] " +
                               "DELETE FROM [usersmi].[RoleClaims] " +
                               "DELETE FROM [usersmi].[Roles] " +
                               "DELETE FROM [usersmi].[UserClaims] " +
                               "DELETE FROM [usersmi].[UserLogins] " +
                               "DELETE FROM [usersmi].[UserRefreshTokens] " +
                               "DELETE FROM [usersmi].[UserTokens] " +
                               "DELETE FROM [usersmi].[Users] " +
                               "DELETE FROM [usersmi].[Permissions] ";

            await connection.ExecuteScalarAsync(sql);
        }
    }
}