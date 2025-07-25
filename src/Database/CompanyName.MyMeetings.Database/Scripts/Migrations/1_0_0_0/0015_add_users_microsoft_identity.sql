CREATE SCHEMA [usersmi]
    AUTHORIZATION [dbo];

GO
CREATE TABLE [usersmi].[UserTokens] (
    [UserId]        UNIQUEIDENTIFIER NOT NULL,
    [LoginProvider] NVARCHAR (450)   NOT NULL,
    [Name]          NVARCHAR (450)   NOT NULL,
    [Value]         NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_UserToken_UserId_LoginProvider_Name] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [Name] ASC)
);

GO
CREATE TABLE [usersmi].[UserRoles] (
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [RoleId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_UserRole_UserId_RoleId] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC)
);

GO
CREATE TABLE [usersmi].[UserRefreshTokens] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [Token]      NVARCHAR (MAX)   NOT NULL,
    [JwtId]      NVARCHAR (MAX)   NOT NULL,
    [IsRevoked]  BIT              NOT NULL,
    [AddedDate]  DATETIME2 (7)    NOT NULL,
    [ExpiryDate] DATETIME2 (7)    NOT NULL,
    CONSTRAINT [PK_UserRefreshToken_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [usersmi].[UserLogins] (
    [LoginProvider]       NVARCHAR (450)   NOT NULL,
    [ProviderKey]         NVARCHAR (450)   NOT NULL,
    [ProviderDisplayName] NVARCHAR (MAX)   NULL,
    [UserId]              UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_UserLogin_LoginProvider_ProviderKey] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC)
);

GO
CREATE TABLE [usersmi].[UserClaims] (
    [Id]         INT              IDENTITY (1, 1) NOT NULL,
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [ClaimType]  NVARCHAR (MAX)   NULL,
    [ClaimValue] NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_UserClaim_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [usersmi].[Users] (
    [Id]                   UNIQUEIDENTIFIER   NOT NULL,
    [Name]                 NVARCHAR (255)     NULL,
    [FirstName]            NVARCHAR (100)     NULL,
    [LastName]             NVARCHAR (100)     NULL,
    [UserName]             NVARCHAR (256)     NULL,
    [NormalizedUserName]   NVARCHAR (256)     NULL,
    [Email]                NVARCHAR (256)     NULL,
    [NormalizedEmail]      NVARCHAR (256)     NULL,
    [EmailConfirmed]       BIT                NOT NULL,
    [PasswordHash]         NVARCHAR (MAX)     NULL,
    [SecurityStamp]        NVARCHAR (MAX)     NULL,
    [ConcurrencyStamp]     NVARCHAR (MAX)     NULL,
    [PhoneNumber]          NVARCHAR (MAX)     NULL,
    [PhoneNumberConfirmed] BIT                NOT NULL,
    [TwoFactorEnabled]     BIT                NOT NULL,
    [LockoutEnd]           DATETIMEOFFSET (7) NULL,
    [LockoutEnabled]       BIT                NOT NULL,
    [AccessFailedCount]    INT                NOT NULL,
    CONSTRAINT [PK_User_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_User_NormalizedUserName] UNIQUE NONCLUSTERED ([NormalizedUserName] ASC),
    CONSTRAINT [UQ_User_UserName] UNIQUE NONCLUSTERED ([UserName] ASC)
);

GO
CREATE TABLE [usersmi].[RoleClaims] (
    [Id]         INT              IDENTITY (1, 1) NOT NULL,
    [RoleId]     UNIQUEIDENTIFIER NOT NULL,
    [ClaimType]  NVARCHAR (MAX)   NULL,
    [ClaimValue] NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_RoleClaim_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [usersmi].[Roles] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [Name]             NVARCHAR (256)   NULL,
    [NormalizedName]   NVARCHAR (256)   NULL,
    [ConcurrencyStamp] NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Role_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Role_Name] UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [UQ_Role_NormalizedName] UNIQUE NONCLUSTERED ([NormalizedName] ASC)
);

GO
CREATE TABLE [usersmi].[Permissions] (
    [Code]        VARCHAR (100) NOT NULL,
    [Name]        VARCHAR (100) NOT NULL,
    [Description] VARCHAR (255) NULL,
    CONSTRAINT [PK_Permission_Code] PRIMARY KEY CLUSTERED ([Code] ASC)
);

GO
CREATE TABLE [usersmi].[OutboxMessages] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [OccurredOn]    DATETIME2 (7)    NOT NULL,
    [Type]          VARCHAR (255)    NOT NULL,
    [Data]          VARCHAR (MAX)    NOT NULL,
    [ProcessedDate] DATETIME2 (7)    NULL,
    CONSTRAINT [PK_OutboxMessages_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [usersmi].[InternalCommands] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [EnqueueDate]   DATETIME2 (7)    NOT NULL,
    [Type]          VARCHAR (255)    NOT NULL,
    [Data]          VARCHAR (MAX)    NOT NULL,
    [ProcessedDate] DATETIME2 (7)    NULL,
    [Error]         NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_InternalCommands_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [usersmi].[InboxMessages] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [OccurredOn]    DATETIME2 (7)    NOT NULL,
    [Type]          VARCHAR (255)    NOT NULL,
    [Data]          VARCHAR (MAX)    NOT NULL,
    [ProcessedDate] DATETIME2 (7)    NULL,
    CONSTRAINT [PK_InboxMessages_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
ALTER TABLE [usersmi].[UserTokens] WITH NOCHECK
    ADD CONSTRAINT [FK_UserToken_UserId_User_Id] FOREIGN KEY ([UserId]) REFERENCES [usersmi].[Users] ([Id]) ON DELETE CASCADE;


GO
ALTER TABLE [usersmi].[UserRoles] WITH NOCHECK
    ADD CONSTRAINT [FK_UserRole_RoleId_Role_Id] FOREIGN KEY ([RoleId]) REFERENCES [usersmi].[Roles] ([Id]) ON DELETE CASCADE;


GO
ALTER TABLE [usersmi].[UserRoles] WITH NOCHECK
    ADD CONSTRAINT [FK_UserRole_UserId_User_Id] FOREIGN KEY ([UserId]) REFERENCES [usersmi].[Users] ([Id]) ON DELETE CASCADE;


GO
ALTER TABLE [usersmi].[UserRefreshTokens] WITH NOCHECK
    ADD CONSTRAINT [FK_UserRefreshToken_UserId_User_Id] FOREIGN KEY ([UserId]) REFERENCES [usersmi].[Users] ([Id]) ON DELETE CASCADE;

GO
ALTER TABLE [usersmi].[UserLogins] WITH NOCHECK
    ADD CONSTRAINT [FK_UserLogin_UserId_User_Id] FOREIGN KEY ([UserId]) REFERENCES [usersmi].[Users] ([Id]) ON DELETE CASCADE;


GO
ALTER TABLE [usersmi].[UserClaims] WITH NOCHECK
    ADD CONSTRAINT [FK_UserClaim_UserId_User_Id] FOREIGN KEY ([UserId]) REFERENCES [usersmi].[Users] ([Id]) ON DELETE CASCADE;


GO
ALTER TABLE [usersmi].[RoleClaims] WITH NOCHECK
    ADD CONSTRAINT [FK_RoleClaim_RoleId_Role_Id] FOREIGN KEY ([RoleId]) REFERENCES [usersmi].[Roles] ([Id]) ON DELETE CASCADE;


GO
CREATE VIEW [usersmi].[v_UserRoles]
AS
SELECT [UserRole].[UserId]	AS [UserId],
       [Role].[Name]		AS [RoleCode]
  FROM [usersmi].[UserRoles] AS [UserRole] 
	   INNER JOIN [usersmi].[Roles] AS [Role]
			   ON [UserRole].[RoleId] = [Role].[Id]
GO

CREATE VIEW [usersmi].[v_UserPermissions]
AS
SELECT 
	DISTINCT
	[UserRole].[UserId]				 AS [UserId],
	[RoleClaim].[ClaimValue]		 AS [PermissionCode]
FROM [usersmi].UserRoles AS [UserRole]
	 INNER JOIN [usersmi].[RoleClaims] AS [RoleClaim]
			 ON [UserRole].[RoleId] = [RoleClaim].[RoleId]
GO

CREATE VIEW [usersmi].[v_Users]
AS
SELECT
    [User].[Id],
    IIF([User].[LockoutEnabled] = 1, 0, 1) AS [IsActive],
    [User].[UserName] AS [Login],
    [User].[PasswordHash] AS [Password],
    [User].[Email],
    [User].[Name]
FROM [usersmi].[Users] AS [User]

GO
ALTER TABLE [usersmi].[UserTokens] WITH CHECK CHECK CONSTRAINT [FK_UserToken_UserId_User_Id];

ALTER TABLE [usersmi].[UserRoles] WITH CHECK CHECK CONSTRAINT [FK_UserRole_RoleId_Role_Id];

ALTER TABLE [usersmi].[UserRoles] WITH CHECK CHECK CONSTRAINT [FK_UserRole_UserId_User_Id];

ALTER TABLE [usersmi].[UserRefreshTokens] WITH CHECK CHECK CONSTRAINT [FK_UserRefreshToken_UserId_User_Id];

ALTER TABLE [usersmi].[UserLogins] WITH CHECK CHECK CONSTRAINT [FK_UserLogin_UserId_User_Id];

ALTER TABLE [usersmi].[UserClaims] WITH CHECK CHECK CONSTRAINT [FK_UserClaim_UserId_User_Id];

ALTER TABLE [usersmi].[RoleClaims] WITH CHECK CHECK CONSTRAINT [FK_RoleClaim_RoleId_Role_Id];

GO