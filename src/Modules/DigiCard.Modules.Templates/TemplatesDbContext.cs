using Microsoft.EntityFrameworkCore;
namespace DigiCard.Modules.Templates;
public sealed class CardTemplate
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = "";
    public int Version { get; private set; }
    public string Accent { get; private set; } = "";
}
public sealed class TemplatesDbContext(DbContextOptions<TemplatesDbContext> options) : DbContext(options)
{
    public DbSet<CardTemplate> Templates => Set<CardTemplate>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("templates");
        var t = modelBuilder.Entity<CardTemplate>();
        t.HasKey(x => x.Id);
        t.Property(x => x.Name).HasMaxLength(120);
        t.Property(x => x.Accent).HasMaxLength(7);
        t.HasData(new { Id = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b0101"), Name = "باغ ایرانی", Version = 1, Accent = "#52796f" },
                  new { Id = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b0102"), Name = "رز و مروارید", Version = 1, Accent = "#a56b7d" });
    }
}
