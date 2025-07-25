namespace CompanyName.MyMeetings.Modules.UsersMI.Sdk;

public static class ApiEndpoints
{
    private const string ApiBase = "/api";

    public static class Authentication
    {
        public const string Login = $"{Base}/login";
        public const string TwoFactorLogin = $"{Base}/two-factor-login";
        public const string RequestForgotPasswordLink = $"{Base}/request-forgot-password-link";
        public const string ResetPassword = $"{Base}/reset-password";
        public const string ExternalLogin = $"{Base}/external-login";
        public const string ExternalLoginCallback = $"{Base}/external-login-callback";
        public const string RefreshToken = $"{Base}/refresh-token";

        private const string Base = $"{ApiBase}/authentication";
    }

    public static class Me
    {
        public const string GetUserAccount = $"{Base}";
        public const string UpdateProfile = $"{Base}/update-profile";
        public const string ChangePassword = $"{Base}/change-password";
        public const string GetAuthenticatorKey = $"{Base}/authenticator-key";
        public const string ChangeEmailAddress = $"{Base}/change-email-address";
        public const string ConfirmEmailAddress = $"{Base}/confirm-email-address";
        public const string RegisterAuthenticator = $"{Base}/register-authenticator";
        public const string RequestChangeEmailAddressToken = $"{Base}/request-change-email-address-token";
        public const string RequestConfirmEmailAddressToken = $"{Base}/request-confirm-email-address-token";

        private const string Base = $"{ApiBase}/me";
    }

    public static class Authorization
    {
        public const string GetPermissions = $"{Base}/permissions";

        private const string Base = $"{ApiBase}/authorization";
    }

    public static class Roles
    {
        public const string GetRoles = Base;
        public const string GetRoleById = $"{Base}/{{roleId}}";
        public const string AddRole = Base;
        public const string RenameRole = $"{Base}/{{roleId}}/rename";
        public const string DeleteRole = $"{Base}/{{roleId}}";
        public const string GetRolePermissions = $"{Base}/{{roleId}}/permissions";
        public const string SetRolePermissions = $"{Base}/{{roleId}}/permissions";

        private const string Base = $"{ApiBase}/users/roles";
    }

    public static class Users
    {
        public const string GetUsers = Base;
        public const string GetUserById = $"{Base}/{{userId}}";
        public const string UpdateUser = $"{Base}/{{userId}}";
        public const string UnlockUser = $"{Base}/{{userId}}/unlock";
        public const string GetUserRoles = $"{Base}/{{userId}}/roles";
        public const string SetUserRoles = $"{Base}/{{userId}}/roles";
        public const string GetUserPermissions = $"{Base}/{{userId}}/permissions";
        public const string SetUserPermissions = $"{Base}/{{userId}}/permissions";
        public const string ChangeUserEmailAddress = $"{Base}/{{userId}}/change-email-address";

        private const string Base = $"{ApiBase}/users/accounts";
    }
}