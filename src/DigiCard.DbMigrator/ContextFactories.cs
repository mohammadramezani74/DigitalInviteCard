using System.Text.Json;
using DigiCard.Modules.Accounts;
using DigiCard.Modules.Templates;
using DigiCard.Modules.Invitations.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DigiCard.DbMigrator;

internal static class DatabaseOptions
{
    /// <summary>
    /// The one and only connection string, read from DigiCard.Web/appsettings.json.
    /// That file is linked into this project and copied next to the executable at build time,
    /// so the migrator and the web app can never drift onto different databases.
    /// </summary>
    private static readonly Lazy<string> Connection = new(ReadConnectionString);

    public static DbContextOptions<T> For<T>(string schema) where T : DbContext =>
        new DbContextOptionsBuilder<T>()
            .UseSqlServer(Connection.Value,
                sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", schema))
            .Options;

    private static string ReadConnectionString()
    {
        var path = FindSettingsFile()
            ?? throw new InvalidOperationException(
                "appsettings.json was not found. It is linked from src/DigiCard.Web and should be " +
                "copied next to the migrator on build; try rebuilding DigiCard.DbMigrator.");

        using var document = JsonDocument.Parse(File.ReadAllText(path), new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        });

        if (document.RootElement.TryGetProperty("ConnectionStrings", out var section) &&
            section.TryGetProperty("DefaultConnection", out var value) &&
            value.GetString() is { Length: > 0 } connection)
        {
            return connection;
        }

        throw new InvalidOperationException(
            $"ConnectionStrings:DefaultConnection is missing or empty in {path}.");
    }

    private static string? FindSettingsFile()
    {
        // Normal case: copied next to the executable.
        var local = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (File.Exists(local)) return local;

        // Fallback for "dotnet ef", which may run from a different working directory:
        // walk up until the web project is found.
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "DigiCard.Web", "appsettings.json");
            if (File.Exists(candidate)) return candidate;
            directory = directory.Parent;
        }

        return null;
    }
}

public sealed class AccountsFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args) => new(DatabaseOptions.For<ApplicationDbContext>("accounts"));
}

public sealed class TemplatesFactory : IDesignTimeDbContextFactory<TemplatesDbContext>
{
    public TemplatesDbContext CreateDbContext(string[] args) => new(DatabaseOptions.For<TemplatesDbContext>("templates"));
}

public sealed class InvitationsFactory : IDesignTimeDbContextFactory<InvitationsDbContext>
{
    public InvitationsDbContext CreateDbContext(string[] args) => new(DatabaseOptions.For<InvitationsDbContext>("invitations"));
}
