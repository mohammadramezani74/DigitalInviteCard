IF OBJECT_ID(N'[invitations].[__EFMigrationsHistory]') IS NULL
BEGIN
    IF SCHEMA_ID(N'invitations') IS NULL EXEC(N'CREATE SCHEMA [invitations];');
    CREATE TABLE [invitations].[__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [invitations].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220802_InitialInvitations'
)
BEGIN
    IF SCHEMA_ID(N'invitations') IS NULL EXEC(N'CREATE SCHEMA [invitations];');
END;

IF NOT EXISTS (
    SELECT * FROM [invitations].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220802_InitialInvitations'
)
BEGIN
    CREATE TABLE [invitations].[Drafts] (
        [Id] uniqueidentifier NOT NULL,
        [OwnerId] nvarchar(450) NOT NULL,
        [Title] nvarchar(100) NOT NULL,
        [BrideName] nvarchar(80) NOT NULL,
        [GroomName] nvarchar(80) NOT NULL,
        [Message] nvarchar(1000) NOT NULL,
        [TemplateId] uniqueidentifier NOT NULL,
        [TemplateVersion] int NOT NULL,
        [Accent] nvarchar(7) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_Drafts] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [invitations].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220802_InitialInvitations'
)
BEGIN
    CREATE INDEX [IX_Drafts_OwnerId_CreatedAt] ON [invitations].[Drafts] ([OwnerId], [CreatedAt]);
END;

IF NOT EXISTS (
    SELECT * FROM [invitations].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220802_InitialInvitations'
)
BEGIN
    INSERT INTO [invitations].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260912220802_InitialInvitations', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [invitations].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213520_DraftElements'
)
BEGIN
    ALTER TABLE [invitations].[Drafts] ADD [Elements] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [invitations].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213520_DraftElements'
)
BEGIN
    INSERT INTO [invitations].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260916213520_DraftElements', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [invitations].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221945_DraftEventDate'
)
BEGIN
    ALTER TABLE [invitations].[Drafts] ADD [EventDate] date NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [invitations].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221945_DraftEventDate'
)
BEGIN
    ALTER TABLE [invitations].[Drafts] ADD [EventTime] time(0) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [invitations].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221945_DraftEventDate'
)
BEGIN
    INSERT INTO [invitations].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260916221945_DraftEventDate', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [invitations].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260917141252_EventEndTime'
)
BEGIN
    ALTER TABLE [invitations].[Drafts] ADD [EventEndTime] time(0) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [invitations].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260917141252_EventEndTime'
)
BEGIN
    INSERT INTO [invitations].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260917141252_EventEndTime', N'10.0.12');
END;

COMMIT;
GO

