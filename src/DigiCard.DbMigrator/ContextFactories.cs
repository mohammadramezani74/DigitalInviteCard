using DigiCard.Modules.Accounts;
using DigiCard.Modules.Templates;
using DigiCard.Modules.Invitations.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace DigiCard.DbMigrator;
internal static class DatabaseOptions
{
    public static DbContextOptions<T> For<T>(string schema) where T : DbContext
    {
        var connection = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Server=.;Database=DigitalCard;User Id=sa;Password=39143914;TrustServerCertificate=True;Integrated Security = False;Encrypt=false";
        return new DbContextOptionsBuilder<T>().UseSqlServer(connection,
            sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", schema)).Options;
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
