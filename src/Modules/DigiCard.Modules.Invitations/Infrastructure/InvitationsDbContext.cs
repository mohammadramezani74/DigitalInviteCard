using DigiCard.Modules.Invitations.Domain;
using Microsoft.EntityFrameworkCore;
namespace DigiCard.Modules.Invitations.Infrastructure;
public sealed class InvitationsDbContext(DbContextOptions<InvitationsDbContext> options) : DbContext(options)
{
    public DbSet<InvitationDraft> Drafts => Set<InvitationDraft>();
    public DbSet<MediaAsset> Media => Set<MediaAsset>();
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
        // nvarchar(max), empty by default. Drafts created before the element model keep "" and
        // fall back to the fixed markup, so no existing row needs rewriting.
        d.Property(x => x.Elements).HasDefaultValue("").IsRequired();
        d.Property(x => x.EventDate).HasColumnType("date");
        d.Property(x => x.EventTime).HasColumnType("time(0)");
        d.Property(x => x.EventEndTime).HasColumnType("time(0)");
        d.Property(x => x.Address).HasMaxLength(300).HasDefaultValue("").IsRequired();
        d.Property(x => x.RowVersion).IsRowVersion();
        // The list page orders by last change, so that is the index it needs.
        d.HasIndex(x => new { x.OwnerId, x.UpdatedAt });

        var m = modelBuilder.Entity<MediaAsset>();
        m.HasKey(x => x.Id);
        m.Property(x => x.OwnerId).HasMaxLength(450).IsRequired();
        m.Property(x => x.Kind).HasMaxLength(20).IsRequired();
        // Every read is "this id, owned by this person", which is exactly this index.
        m.HasIndex(x => new { x.OwnerId, x.CreatedAt });
    }
}
