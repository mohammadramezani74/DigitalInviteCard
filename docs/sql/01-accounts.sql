IF OBJECT_ID(N'[accounts].[__EFMigrationsHistory]') IS NULL
BEGIN
    IF SCHEMA_ID(N'accounts') IS NULL EXEC(N'CREATE SCHEMA [accounts];');
    CREATE TABLE [accounts].[__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    IF SCHEMA_ID(N'accounts') IS NULL EXEC(N'CREATE SCHEMA [accounts];');
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    CREATE TABLE [accounts].[AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    CREATE TABLE [accounts].[AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    CREATE TABLE [accounts].[AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [accounts].[AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    CREATE TABLE [accounts].[AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [accounts].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    CREATE TABLE [accounts].[AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [accounts].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    CREATE TABLE [accounts].[AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [accounts].[AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [accounts].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    CREATE TABLE [accounts].[AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [accounts].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [accounts].[AspNetRoleClaims] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [accounts].[AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [accounts].[AspNetUserClaims] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [accounts].[AspNetUserLogins] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [accounts].[AspNetUserRoles] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [accounts].[AspNetUsers] ([NormalizedEmail]);
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [accounts].[AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [accounts].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220713_InitialAccounts'
)
BEGIN
    INSERT INTO [accounts].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260912220713_InitialAccounts', N'10.0.12');
END;

COMMIT;
GO

