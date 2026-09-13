using DigiCard.Modules.Invitations.Domain;
using Microsoft.EntityFrameworkCore;
namespace DigiCard.Modules.Invitations.Infrastructure;
public sealed class InvitationsDbContext(DbContextOptions<InvitationsDbContext> options) : DbContext(options)
{
    public DbSet<InvitationDraft> Drafts => Set<InvitationDraft>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("invitations");
        var d = modelBuilder.Entity<InvitationDraft>();
        d.HasKey(x => x.Id);
        d.Property(x => x.OwnerId).HasMaxLength(450).IsRequired();
        d.Property(x => x.Title).HasMaxLength(100).IsRequired();
        d.Property(x => x.BrideName).HasMaxLength(80).IsRequired();
        d.Property(x => x.GroomName).HasMaxLength(80).IsRequired();
        d.Property(x => x.Message).HasMaxLength(1000);
        d.Property(x => x.Accent).HasMaxLength(7);
        d.Property(x => x.RowVersion).IsRowVersion();
        d.HasIndex(x => new { x.OwnerId, x.CreatedAt });
    }
}
