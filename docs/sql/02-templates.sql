IF OBJECT_ID(N'[templates].[__EFMigrationsHistory]') IS NULL
BEGIN
    IF SCHEMA_ID(N'templates') IS NULL EXEC(N'CREATE SCHEMA [templates];');
    CREATE TABLE [templates].[__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220743_InitialTemplates'
)
BEGIN
    IF SCHEMA_ID(N'templates') IS NULL EXEC(N'CREATE SCHEMA [templates];');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220743_InitialTemplates'
)
BEGIN
    CREATE TABLE [templates].[Templates] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(120) NOT NULL,
        [Version] int NOT NULL,
        [Accent] nvarchar(7) NOT NULL,
        CONSTRAINT [PK_Templates] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220743_InitialTemplates'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Accent', N'Name', N'Version') AND [object_id] = OBJECT_ID(N'[templates].[Templates]'))
        SET IDENTITY_INSERT [templates].[Templates] ON;
    EXEC(N'INSERT INTO [templates].[Templates] ([Id], [Accent], [Name], [Version])
    VALUES (''a41a6319-3438-4ef9-89da-fae6b30b0101'', N''#52796f'', N''باغ ایرانی'', 1),
    (''a41a6319-3438-4ef9-89da-fae6b30b0102'', N''#a56b7d'', N''رز و مروارید'', 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Accent', N'Name', N'Version') AND [object_id] = OBJECT_ID(N'[templates].[Templates]'))
        SET IDENTITY_INSERT [templates].[Templates] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260912220743_InitialTemplates'
)
BEGIN
    INSERT INTO [templates].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260912220743_InitialTemplates', N'10.0.12');
END;

COMMIT;
GO

