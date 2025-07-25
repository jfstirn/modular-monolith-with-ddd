namespace CompanyName.MyMeetings.Modules.UsersMI.WebApi.Endpoints;

internal class UsersPermissions
{
    // Me
    // No permissions needed for these endpoints. Just check if the user is authenticated.

    // Users
    public const string GetUsers = "Users.GetUsers";
    public const string UpdateUserAccount = "Users.UpdateUserAccount";
    public const string UnlockUserAccount = "Users.UnlockUserAccount";
    public const string ConfirmEmailAddress = "Users.ConfirmEmailAddress";
    public const string GetAuthenticatorKey = "Users.GetAuthenticatorKey";
    public const string RegisterAuthenticator = "Users.RegisterAuthenticator";
    public const string GetUserRoles = "Users.GetUserRoles";
    public const string SetUserRoles = "Users.SetUserRoles";
    public const string GetUserPermissions = "Users.GetUserPermissions";
    public const string SetUserPermissions = "Users.SetUserPermissions";
    public const string ChangeUserEmailAddress = "Users.ChangeUserEmailAddress";

    // Roles
    public const string GetRoles = "Users.GetRoles";
    public const string AddRole = "Users.AddRole";
    public const string RenameRole = "Users.RenameRole";
    public const string DeleteRole = "Users.DeleteRole";
    public const string GetRolePermissions = "Users.GetRolePermissions";
    public const string SetRolePermissions = "Users.SetRolePermissions";
}