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

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    ALTER TABLE [templates].[Templates] ADD [Background] nvarchar(240) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    ALTER TABLE [templates].[Templates] ADD [Category] nvarchar(40) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    ALTER TABLE [templates].[Templates] ADD [Family] nvarchar(40) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    ALTER TABLE [templates].[Templates] ADD [Frame] nvarchar(20) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    ALTER TABLE [templates].[Templates] ADD [Ink] nvarchar(7) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    ALTER TABLE [templates].[Templates] ADD [Layout] nvarchar(20) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    ALTER TABLE [templates].[Templates] ADD [Ornament] nvarchar(20) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    ALTER TABLE [templates].[Templates] ADD [Slug] nvarchar(60) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    ALTER TABLE [templates].[Templates] ADD [Typeface] nvarchar(20) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Background] = N''linear-gradient(180deg,#f4f7f3 0%,#e9f0ea 100%)'', [Category] = N''wedding'', [Family] = N''persian'', [Frame] = N''ornate'', [Ink] = N''#2b3b35'', [Layout] = N''arch'', [Ornament] = N''paisley'', [Slug] = N''bagh-e-irani'', [Typeface] = N''display'', [Version] = 2
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0101'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Background] = N''linear-gradient(180deg,#fbf4f6 0%,#f2e6ea 100%)'', [Category] = N''wedding'', [Family] = N''photo'', [Frame] = N''thin'', [Ink] = N''#3d2f34'', [Layout] = N''photo'', [Ornament] = N''dot'', [Slug] = N''roz-o-morvarid'', [Typeface] = N''serif'', [Version] = 2
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0102'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Accent', N'Background', N'Category', N'Family', N'Frame', N'Ink', N'Layout', N'Name', N'Ornament', N'Slug', N'Typeface', N'Version') AND [object_id] = OBJECT_ID(N'[templates].[Templates]'))
        SET IDENTITY_INSERT [templates].[Templates] ON;
    EXEC(N'INSERT INTO [templates].[Templates] ([Id], [Accent], [Background], [Category], [Family], [Frame], [Ink], [Layout], [Name], [Ornament], [Slug], [Typeface], [Version])
    VALUES (''a41a6319-3438-4ef9-89da-fae6b30b0103'', N''#8a7f6b'', N''linear-gradient(180deg,#fffdf9 0%,#f6f1e8 100%)'', N''wedding'', N''minimal'', N''thin'', N''#3b3a35'', N''centered'', N''سپید ساده'', N''rule'', N''sepid-sade'', N''serif'', 1),
    (''a41a6319-3438-4ef9-89da-fae6b30b0104'', N''#9a8c74'', N''#fbfaf7'', N''aghd'', N''minimal'', N''none'', N''#35332e'', N''banded'', N''خط نور'', N''dot'', N''khat-e-noor'', N''sans'', 1),
    (''a41a6319-3438-4ef9-89da-fae6b30b0105'', N''#b08d57'', N''linear-gradient(180deg,#fdfbf6 0%,#f4ecdd 100%)'', N''anniversary'', N''minimal'', N''thin'', N''#37352f'', N''centered'', N''سادگی طلایی'', N''rule'', N''sadegi-talaei'', N''display'', 1),
    (''a41a6319-3438-4ef9-89da-fae6b30b0106'', N''#c17f8f'', N''radial-gradient(120% 80% at 50% 0%,#fdeef1 0%,#fbf7f4 55%,#f7eee9 100%)'', N''wedding'', N''watercolor'', N''none'', N''#4a3a3e'', N''arch'', N''گلاب آبرنگ'', N''floral'', N''golab-abrang'', N''display'', 1),
    (''a41a6319-3438-4ef9-89da-fae6b30b0107'', N''#c4636b'', N''radial-gradient(100% 70% at 20% 10%,#fdeceb 0%,#fcf8f5 60%,#f8f1ec 100%)'', N''engagement'', N''watercolor'', N''none'', N''#46333a'', N''centered'', N''شقایق'', N''floral'', N''shaghayegh'', N''serif'', 1),
    (''a41a6319-3438-4ef9-89da-fae6b30b0108'', N''#7d6ea8'', N''radial-gradient(110% 80% at 80% 0%,#f1edfa 0%,#faf8fc 55%,#f4f1f7 100%)'', N''baleboron'', N''watercolor'', N''none'', N''#3b3548'', N''arch'', N''بنفشه'', N''floral'', N''banafsheh'', N''display'', 1),
    (''a41a6319-3438-4ef9-89da-fae6b30b0109'', N''#c98a3e'', N''linear-gradient(160deg,#fdf4e4 0%,#fbf6ee 55%,#f6ead6 100%)'', N''hanabandan'', N''watercolor'', N''none'', N''#45362a'', N''banded'', N''حنا و گل'', N''floral'', N''hana-o-gol'', N''display'', 1),
    (''a41a6319-3438-4ef9-89da-fae6b30b010a'', N''#8c3b3b'', N''linear-gradient(180deg,#fbf3ee 0%,#f3e4dc 100%)'', N''aghd'', N''persian'', N''ornate'', N''#3a2424'', N''banded'', N''ترمه'', N''paisley'', N''termeh'', N''display'', 1),
    (''a41a6319-3438-4ef9-89da-fae6b30b010b'', N''#2e7d8c'', N''linear-gradient(180deg,#eff7f8 0%,#e2eff1 100%)'', N''hanabandan'', N''persian'', N''ornate'', N''#23383d'', N''centered'', N''کاشی فیروزه'', N''geometric'', N''kashi-firouzeh'', N''serif'', 1),
    (''a41a6319-3438-4ef9-89da-fae6b30b010c'', N''#6b6f7d'', N''linear-gradient(180deg,#f6f7f9 0%,#e9ebef 100%)'', N''anniversary'', N''photo'', N''double'', N''#2f3138'', N''photo'', N''قاب خاطره'', N''rule'', N''ghab-e-khatereh'', N''sans'', 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Accent', N'Background', N'Category', N'Family', N'Frame', N'Ink', N'Layout', N'Name', N'Ornament', N'Slug', N'Typeface', N'Version') AND [object_id] = OBJECT_ID(N'[templates].[Templates]'))
        SET IDENTITY_INSERT [templates].[Templates] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    CREATE INDEX [IX_Templates_Category] ON [templates].[Templates] ([Category]);
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Templates_Slug] ON [templates].[Templates] ([Slug]);
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913220133_TemplateDesignTokens'
)
BEGIN
    INSERT INTO [templates].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260913220133_TemplateDesignTokens', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    ALTER TABLE [templates].[Templates] ADD [Artwork] nvarchar(60) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    ALTER TABLE [templates].[Templates] ADD [SafeBottom] int NOT NULL DEFAULT 75;
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    ALTER TABLE [templates].[Templates] ADD [SafeTop] int NOT NULL DEFAULT 30;
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Artwork] = N'''', [SafeBottom] = 75, [SafeTop] = 30
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0101'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Artwork] = N'''', [SafeBottom] = 75, [SafeTop] = 30
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0102'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Artwork] = N'''', [SafeBottom] = 75, [SafeTop] = 30
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0103'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Artwork] = N'''', [SafeBottom] = 75, [SafeTop] = 30
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0104'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Artwork] = N'''', [SafeBottom] = 75, [SafeTop] = 30
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0105'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Artwork] = N''golab-abrang'', [Layout] = N''centered'', [SafeBottom] = 67, [SafeTop] = 37, [Typeface] = N''serif''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0106'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Artwork] = N'''', [SafeBottom] = 75, [SafeTop] = 30
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0107'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Artwork] = N'''', [SafeBottom] = 75, [SafeTop] = 30
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0108'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Artwork] = N'''', [SafeBottom] = 75, [SafeTop] = 30
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0109'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Artwork] = N'''', [SafeBottom] = 75, [SafeTop] = 30
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b010a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Artwork] = N'''', [SafeBottom] = 75, [SafeTop] = 30
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b010b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Artwork] = N'''', [SafeBottom] = 75, [SafeTop] = 30
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b010c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260913223819_TemplateArtwork'
)
BEGIN
    INSERT INTO [templates].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260913223819_TemplateArtwork', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260915205141_TemplateArtworkAll'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Accent] = N''#918063'', [Artwork] = N''bagh-e-irani'', [Frame] = N''none'', [Ink] = N''#3c3a36'', [Layout] = N''centered'', [SafeBottom] = 67, [SafeTop] = 31, [Typeface] = N''serif''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0101'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260915205141_TemplateArtworkAll'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Accent] = N''#8a7c5c'', [Artwork] = N''roz-o-morvarid'', [Frame] = N''none'', [Ink] = N''#3c3a36'', [Layout] = N''centered'', [SafeBottom] = 62, [SafeTop] = 26
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0102'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260915205141_TemplateArtworkAll'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Accent] = N''#847b57'', [Artwork] = N''sepid-sade'', [Frame] = N''none'', [Ink] = N''#3c3a36'', [SafeBottom] = 80, [SafeTop] = 44, [Version] = 2
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0103'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260915205141_TemplateArtworkAll'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Accent] = N''#7c764f'', [Artwork] = N''khat-e-noor'', [Ink] = N''#3c3a36'', [Layout] = N''centered'', [SafeBottom] = 58, [SafeTop] = 22, [Typeface] = N''display'', [Version] = 2
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0104'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260915205141_TemplateArtworkAll'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Accent] = N''#807547'', [Artwork] = N''sadegi-talaei'', [Frame] = N''none'', [Ink] = N''#3c3a36'', [SafeBottom] = 58, [SafeTop] = 22, [Version] = 2
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0105'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260915205141_TemplateArtworkAll'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Accent] = N''#85785d'', [Artwork] = N''shaghayegh'', [Ink] = N''#3c3a36'', [SafeBottom] = 76, [SafeTop] = 40, [Version] = 2
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0107'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260915205141_TemplateArtworkAll'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Accent] = N''#6d5c7f'', [Artwork] = N''banafsheh'', [Ink] = N''#3c3a36'', [Layout] = N''centered'', [SafeBottom] = 81, [SafeTop] = 45, [Typeface] = N''serif'', [Version] = 2
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0108'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260915205141_TemplateArtworkAll'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Accent] = N''#80673d'', [Artwork] = N''hana-o-gol'', [Ink] = N''#3c3a36'', [Layout] = N''centered'', [SafeBottom] = 50, [SafeTop] = 14, [Typeface] = N''serif'', [Version] = 2
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0109'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260915205141_TemplateArtworkAll'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Accent] = N''#566c84'', [Artwork] = N''kashi-firouzeh'', [Frame] = N''none'', [Ink] = N''#3c3a36'', [SafeBottom] = 70, [SafeTop] = 34, [Typeface] = N''display'', [Version] = 2
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b010b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260915205141_TemplateArtworkAll'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Accent] = N''#7d6143'', [Artwork] = N''ghab-e-khatereh'', [Frame] = N''none'', [Ink] = N''#3c3a36'', [Layout] = N''centered'', [SafeBottom] = 69, [SafeTop] = 33, [Typeface] = N''serif'', [Version] = 2
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b010c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260915205141_TemplateArtworkAll'
)
BEGIN
    INSERT INTO [templates].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260915205141_TemplateArtworkAll', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260915210511_RemoveTermehTemplate'
)
BEGIN
    EXEC(N'DELETE FROM [templates].[Templates]
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b010a'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260915210511_RemoveTermehTemplate'
)
BEGIN
    INSERT INTO [templates].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260915210511_RemoveTermehTemplate', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916210254_Occasions'
)
BEGIN
    CREATE TABLE [templates].[Occasions] (
        [Id] uniqueidentifier NOT NULL,
        [Slug] nvarchar(40) NOT NULL,
        [Title] nvarchar(60) NOT NULL,
        [Tagline] nvarchar(120) NOT NULL,
        [Icon] nvarchar(40) NOT NULL,
        [SortOrder] int NOT NULL,
        [IsActive] bit NOT NULL,
        [PrimaryLabel] nvarchar(60) NOT NULL,
        [SecondaryLabel] nvarchar(60) NOT NULL,
        [Joiner] nvarchar(10) NOT NULL,
        [DefaultKicker] nvarchar(120) NOT NULL,
        [DefaultMessage] nvarchar(400) NOT NULL,
        CONSTRAINT [PK_Occasions] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916210254_Occasions'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DefaultKicker', N'DefaultMessage', N'Icon', N'IsActive', N'Joiner', N'PrimaryLabel', N'SecondaryLabel', N'Slug', N'SortOrder', N'Tagline', N'Title') AND [object_id] = OBJECT_ID(N'[templates].[Occasions]'))
        SET IDENTITY_INSERT [templates].[Occasions] ON;
    EXEC(N'INSERT INTO [templates].[Occasions] ([Id], [DefaultKicker], [DefaultMessage], [Icon], [IsActive], [Joiner], [PrimaryLabel], [SecondaryLabel], [Slug], [SortOrder], [Tagline], [Title])
    VALUES (''b7c41f20-0000-4000-9000-000000000001'', N''آغاز یک زندگی، کنار هم'', N''با حضور شما شادی ما کامل می‌شود'', N''wedding'', CAST(1 AS bit), N''و'', N''نام عروس'', N''نام داماد'', N''wedding'', 1, N''دعوت‌نامه جشن عروسی'', N''عروسی''),
    (''b7c41f20-0000-4000-9000-000000000002'', N''پیوند دو دل'', N''با حضور شما شادی ما کامل می‌شود'', N''aghd'', CAST(1 AS bit), N''و'', N''نام عروس'', N''نام داماد'', N''aghd'', 2, N''دعوت به مراسم عقد'', N''عقد''),
    (''b7c41f20-0000-4000-9000-000000000003'', N''آغاز یک قرار'', N''به جشن نامزدی ما خوش آمدید'', N''engagement'', CAST(1 AS bit), N''و'', N''نام عروس'', N''نام داماد'', N''engagement'', 3, N''جشن نامزدی و حلقه'', N''نامزدی''),
    (''b7c41f20-0000-4000-9000-000000000004'', N''یک بله، یک آغاز'', N''در این روز خوش کنار ما باشید'', N''baleboron'', CAST(1 AS bit), N''و'', N''نام عروس'', N''نام داماد'', N''baleboron'', 4, N''مراسم بله‌برون'', N''بله‌برون''),
    (''b7c41f20-0000-4000-9000-000000000005'', N''شب حنا، شب شادی'', N''در شب حنابندان منتظر شما هستیم'', N''hanabandan'', CAST(1 AS bit), N''و'', N''نام عروس'', N''نام داماد'', N''hanabandan'', 5, N''شب حنابندان'', N''حنابندان''),
    (''b7c41f20-0000-4000-9000-000000000006'', N''سال‌هایی که گذشت'', N''در جشن سالگرد ما شریک باشید'', N''anniversary'', CAST(1 AS bit), N''و'', N''نام همسر اول'', N''نام همسر دوم'', N''anniversary'', 6, N''سالگرد و تجدید پیمان'', N''سالگرد ازدواج''),
    (''b7c41f20-0000-4000-9000-000000000007'', N''یاد او همیشه با ماست'', N''به مراسم یادبود ایشان دعوت می‌شوید'', N''memorial'', CAST(0 AS bit), N'''', N''نام درگذشته'', N'''', N''memorial'', 7, N''یادبود و مراسم ترحیم'', N''مجلس ترحیم''),
    (''b7c41f20-0000-4000-9000-000000000008'', N''یک سال تازه'', N''به جشن تولد دعوتید'', N''birthday'', CAST(0 AS bit), N'''', N''نام صاحب جشن'', N'''', N''birthday'', 8, N''جشن تولد'', N''تولد''),
    (''b7c41f20-0000-4000-9000-000000000009'', N''برای پدر'', N''روزت مبارک'', N''fathers-day'', CAST(0 AS bit), N'''', N''نام پدر'', N'''', N''fathers-day'', 9, N''تبریک روز پدر'', N''روز پدر''),
    (''b7c41f20-0000-4000-9000-000000000010'', N''برای مادر'', N''روزت مبارک'', N''mothers-day'', CAST(0 AS bit), N'''', N''نام مادر'', N'''', N''mothers-day'', 10, N''تبریک روز مادر'', N''روز مادر''),
    (''b7c41f20-0000-4000-9000-000000000011'', N''روز دانشجو'', N''گرامی باد'', N''students-day'', CAST(0 AS bit), N'''', N''نام مناسبت'', N'''', N''students-day'', 11, N''گرامیداشت روز دانشجو'', N''روز دانشجو'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DefaultKicker', N'DefaultMessage', N'Icon', N'IsActive', N'Joiner', N'PrimaryLabel', N'SecondaryLabel', N'Slug', N'SortOrder', N'Tagline', N'Title') AND [object_id] = OBJECT_ID(N'[templates].[Occasions]'))
        SET IDENTITY_INSERT [templates].[Occasions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916210254_Occasions'
)
BEGIN
    CREATE INDEX [IX_Occasions_IsActive_SortOrder] ON [templates].[Occasions] ([IsActive], [SortOrder]);
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916210254_Occasions'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Occasions_Slug] ON [templates].[Occasions] ([Slug]);
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916210254_Occasions'
)
BEGIN
    INSERT INTO [templates].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260916210254_Occasions', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213516_CardElements'
)
BEGIN
    ALTER TABLE [templates].[Templates] ADD [Elements] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213516_CardElements'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":31.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1}},{"id":"names","role":"names","x":6,"y":38.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1}},{"id":"divider","role":"divider","x":32,"y":51.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8}},{"id":"message","role":"message","x":12,"y":55.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0101'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213516_CardElements'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":26.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1}},{"id":"names","role":"names","x":6,"y":33.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1}},{"id":"divider","role":"divider","x":32,"y":46.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8}},{"id":"message","role":"message","x":12,"y":50.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0102'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213516_CardElements'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":44.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1}},{"id":"names","role":"names","x":6,"y":51.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1}},{"id":"divider","role":"divider","x":32,"y":64.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8}},{"id":"message","role":"message","x":12,"y":68.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0103'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213516_CardElements'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":22.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1}},{"id":"names","role":"names","x":6,"y":29.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1}},{"id":"divider","role":"divider","x":32,"y":42.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8}},{"id":"message","role":"message","x":12,"y":46.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0104'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213516_CardElements'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":22.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1}},{"id":"names","role":"names","x":6,"y":29.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1}},{"id":"divider","role":"divider","x":32,"y":42.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8}},{"id":"message","role":"message","x":12,"y":46.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0105'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213516_CardElements'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":37.6,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1}},{"id":"names","role":"names","x":6,"y":43,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1}},{"id":"divider","role":"divider","x":32,"y":53.8,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8}},{"id":"message","role":"message","x":12,"y":57.4,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0106'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213516_CardElements'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":40.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1}},{"id":"names","role":"names","x":6,"y":47.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1}},{"id":"divider","role":"divider","x":32,"y":60.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8}},{"id":"message","role":"message","x":12,"y":64.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0107'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213516_CardElements'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":45.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1}},{"id":"names","role":"names","x":6,"y":52.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1}},{"id":"divider","role":"divider","x":32,"y":65.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8}},{"id":"message","role":"message","x":12,"y":69.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0108'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213516_CardElements'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":14.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1}},{"id":"names","role":"names","x":6,"y":21.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1}},{"id":"divider","role":"divider","x":32,"y":34.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8}},{"id":"message","role":"message","x":12,"y":38.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0109'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213516_CardElements'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":34.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1}},{"id":"names","role":"names","x":6,"y":41.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1}},{"id":"divider","role":"divider","x":32,"y":54.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8}},{"id":"message","role":"message","x":12,"y":58.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b010b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213516_CardElements'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":33.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1}},{"id":"names","role":"names","x":6,"y":40.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1}},{"id":"divider","role":"divider","x":32,"y":53.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8}},{"id":"message","role":"message","x":12,"y":57.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b010c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916213516_CardElements'
)
BEGIN
    INSERT INTO [templates].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260916213516_CardElements', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    CREATE TABLE [templates].[Poems] (
        [Id] uniqueidentifier NOT NULL,
        [Text] nvarchar(400) NOT NULL,
        [Poet] nvarchar(60) NOT NULL,
        [OccasionSlug] nvarchar(40) NOT NULL,
        [SortOrder] int NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Poems] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'IsActive', N'OccasionSlug', N'Poet', N'SortOrder', N'Text') AND [object_id] = OBJECT_ID(N'[templates].[Poems]'))
        SET IDENTITY_INSERT [templates].[Poems] ON;
    EXEC(N'INSERT INTO [templates].[Poems] ([Id], [IsActive], [OccasionSlug], [Poet], [SortOrder], [Text])
    VALUES (''c3d52a10-0000-4000-9000-000000000001'', CAST(1 AS bit), N'''', N''حافظ'', 1, CONCAT(CAST(N''درخت دوستی بنشان که کام دل به بار آرد'' AS nvarchar(max)), nchar(10), N''نهال دشمنی برکن که رنج بی‌شمار آرد'')),
    (''c3d52a10-0000-4000-9000-000000000002'', CAST(1 AS bit), N'''', N''حافظ'', 2, CONCAT(CAST(N''دست از طلب ندارم تا کام من برآید'' AS nvarchar(max)), nchar(10), N''یا تن رسد به جانان یا جان ز تن برآید'')),
    (''c3d52a10-0000-4000-9000-000000000003'', CAST(1 AS bit), N'''', N''حافظ'', 3, CONCAT(CAST(N''مرا مهر سیه‌چشمان ز سر بیرون نخواهد شد'' AS nvarchar(max)), nchar(10), N''قضای آسمان است این و دیگرگون نخواهد شد'')),
    (''c3d52a10-0000-4000-9000-000000000004'', CAST(1 AS bit), N'''', N''سعدی'', 4, CONCAT(CAST(N''به جهان خرم از آنم که جهان خرم از اوست'' AS nvarchar(max)), nchar(10), N''عاشقم بر همه عالم که همه عالم از اوست'')),
    (''c3d52a10-0000-4000-9000-000000000005'', CAST(1 AS bit), N'''', N''سعدی'', 5, CONCAT(CAST(N''بنی‌آدم اعضای یک پیکرند'' AS nvarchar(max)), nchar(10), N''که در آفرینش ز یک گوهرند'')),
    (''c3d52a10-0000-4000-9000-000000000006'', CAST(1 AS bit), N'''', N''مولوی'', 6, CONCAT(CAST(N''عشق آن شعله است کاو چون برفروخت'' AS nvarchar(max)), nchar(10), N''هر چه جز معشوق باقی جمله سوخت'')),
    (''c3d52a10-0000-4000-9000-000000000007'', CAST(1 AS bit), N'''', N''نظامی'', 7, CONCAT(CAST(N''به نام آنکه جان را فکرت آموخت'' AS nvarchar(max)), nchar(10), N''چراغ دل به نور جان برافروخت''))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'IsActive', N'OccasionSlug', N'Poet', N'SortOrder', N'Text') AND [object_id] = OBJECT_ID(N'[templates].[Poems]'))
        SET IDENTITY_INSERT [templates].[Poems] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":31.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"names","role":"names","x":6,"y":38.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"divider","role":"divider","x":32,"y":51.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8,"variant":"rule","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"message","role":"message","x":12,"y":55.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0101'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":26.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"names","role":"names","x":6,"y":33.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"divider","role":"divider","x":32,"y":46.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8,"variant":"rule","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"message","role":"message","x":12,"y":50.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0102'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":44.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"names","role":"names","x":6,"y":51.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"divider","role":"divider","x":32,"y":64.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8,"variant":"rule","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"message","role":"message","x":12,"y":68.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0103'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":22.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"names","role":"names","x":6,"y":29.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"divider","role":"divider","x":32,"y":42.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8,"variant":"rule","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"message","role":"message","x":12,"y":46.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0104'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":22.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"names","role":"names","x":6,"y":29.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"divider","role":"divider","x":32,"y":42.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8,"variant":"rule","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"message","role":"message","x":12,"y":46.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0105'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":37.6,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"names","role":"names","x":6,"y":43,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"divider","role":"divider","x":32,"y":53.8,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8,"variant":"rule","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"message","role":"message","x":12,"y":57.4,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0106'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":40.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"names","role":"names","x":6,"y":47.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"divider","role":"divider","x":32,"y":60.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8,"variant":"rule","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"message","role":"message","x":12,"y":64.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0107'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":45.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"names","role":"names","x":6,"y":52.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"divider","role":"divider","x":32,"y":65.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8,"variant":"rule","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"message","role":"message","x":12,"y":69.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0108'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":14.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"names","role":"names","x":6,"y":21.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"divider","role":"divider","x":32,"y":34.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8,"variant":"rule","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"message","role":"message","x":12,"y":38.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b0109'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":34.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"names","role":"names","x":6,"y":41.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"divider","role":"divider","x":32,"y":54.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8,"variant":"rule","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"message","role":"message","x":12,"y":58.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b010b'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    EXEC(N'UPDATE [templates].[Templates] SET [Elements] = N''[{"id":"kicker","role":"kicker","x":10,"y":33.72,"w":80,"rotation":0,"z":10,"style":{"fontSize":3.2,"font":"body","weight":"normal","align":"center","color":"muted","lineHeight":1.6,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"names","role":"names","x":6,"y":40.2,"w":88,"rotation":0,"z":20,"style":{"fontSize":8.4,"font":"display","weight":"bold","align":"center","color":"ink","lineHeight":1.35,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"divider","role":"divider","x":32,"y":53.16,"w":36,"h":3,"rotation":0,"z":15,"style":{"fontSize":4,"font":"body","weight":"normal","align":"center","color":"accent","lineHeight":1.7,"opacity":0.8,"variant":"rule","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}},{"id":"message","role":"message","x":12,"y":57.48,"w":76,"rotation":0,"z":10,"style":{"fontSize":3.6,"font":"body","weight":"normal","align":"center","color":"ink","lineHeight":1.9,"opacity":1,"variant":"plain","digits":"fa","frame":"none","feather":0,"hasLiteralColor":false}}]''
    WHERE [Id] = ''a41a6319-3438-4ef9-89da-fae6b30b010c'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    CREATE INDEX [IX_Poems_IsActive_OccasionSlug_SortOrder] ON [templates].[Poems] ([IsActive], [OccasionSlug], [SortOrder]);
END;

IF NOT EXISTS (
    SELECT * FROM [templates].[__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260916221942_CardVariants'
)
BEGIN
    INSERT INTO [templates].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260916221942_CardVariants', N'10.0.12');
END;

COMMIT;
GO

