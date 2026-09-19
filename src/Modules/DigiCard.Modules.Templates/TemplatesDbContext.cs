using DigiCard.Contracts.Templates;
using Microsoft.EntityFrameworkCore;

namespace DigiCard.Modules.Templates;

/// <summary>
/// A catalog entry with design tokens. Tokens are a deliberate middle step: enough to make
/// genuinely distinct cards without a layer engine. They are authored data, never user input,
/// and are rendered into CSS custom properties by CardPreview. If templates ever become
/// user-editable, every token below needs validation before it reaches a style attribute.
/// </summary>
public sealed class CardTemplate
{
    public Guid Id { get; private set; }
    public string Slug { get; private set; } = "";
    public string Name { get; private set; } = "";
    public int Version { get; private set; }

    /// <summary>Occasion slug. Matches Occasion.Slug in the same schema.</summary>
    public string Category { get; private set; } = "";

    /// <summary>Design family: minimal | watercolor | persian | photo.</summary>
    public string Family { get; private set; } = "";

    /// <summary>Primary colour, a #rrggbb value.</summary>
    public string Accent { get; private set; } = "";

    /// <summary>Card ground: any CSS colour or gradient.</summary>
    public string Background { get; private set; } = "";

    /// <summary>Body text colour, a #rrggbb value.</summary>
    public string Ink { get; private set; } = "";

    /// <summary>Border treatment: none | thin | double | ornate.</summary>
    public string Frame { get; private set; } = "";

    /// <summary>Ornament motif: none | rule | dot | floral | paisley | geometric.</summary>
    public string Ornament { get; private set; } = "";

    /// <summary>Typeface role: sans | serif | display.</summary>
    public string Typeface { get; private set; } = "";

    /// <summary>Composition: centered | banded | arch | photo.</summary>
    public string Layout { get; private set; } = "";

    /// <summary>
    /// Folder name under wwwroot/card-art holding this template's painted artwork.
    /// Empty means no artwork has been produced yet, and the vector fallback is drawn instead,
    /// so templates can be filled in one at a time without breaking the gallery.
    /// </summary>
    public string Artwork { get; private set; } = "";

    /// <summary>Top of the readable band, as a percentage of card height.</summary>
    public int SafeTop { get; private set; }

    /// <summary>Bottom of the readable band, as a percentage of card height.</summary>
    public int SafeBottom { get; private set; }

    /// <summary>
    /// The template's arrangement, as the JSON documented on CardLayout: a list of positioned
    /// elements rather than a fixed kicker/names/message stack. This is what the editor edits and
    /// what a new card starts from.
    ///
    /// Empty means "no layout saved yet", and the renderer falls back to the old fixed markup, so
    /// templates can be converted one at a time without the gallery going blank in between.
    /// </summary>
    public string Elements { get; private set; } = "";
}

/// <summary>
/// An occasion a card is made for. Used to be a hardcoded list; it is data so that adding
/// "روز معلم" is an admin action rather than a deployment.
///
/// The label columns carry the tone: a funeral card must never ask for a bride's name, and a
/// birthday card has one name rather than two. Every occasion-dependent word the visitor sees
/// is read from here.
/// </summary>
public sealed class Occasion
{
    public Guid Id { get; private set; }
    public string Slug { get; private set; } = "";
    public string Title { get; private set; } = "";
    public string Tagline { get; private set; } = "";

    /// <summary>Icon key, matching the switch in CategoryIcon.razor.</summary>
    public string Icon { get; private set; } = "";

    public int SortOrder { get; private set; }

    /// <summary>Hidden from the site while false. New occasions start hidden until they have templates.</summary>
    public bool IsActive { get; private set; }

    public string PrimaryLabel { get; private set; } = "";

    /// <summary>Empty when the occasion names only one person.</summary>
    public string SecondaryLabel { get; private set; } = "";

    /// <summary>The word between two names. Empty when there is only one name.</summary>
    public string Joiner { get; private set; } = "";

    public string DefaultKicker { get; private set; } = "";
    public string DefaultMessage { get; private set; } = "";
}

/// <summary>
/// A verse the editor offers in its poem picker.
///
/// Classical, public-domain poetry only - see PoemSummary for why that line is drawn where it is.
/// Attributions were checked rather than guessed; a misattributed verse printed on a thousand
/// wedding cards is not a bug anyone can fix afterwards.
/// </summary>
public sealed class Poem
{
    public Guid Id { get; private set; }

    /// <summary>The verse. Newlines separate hemistichs and survive to the rendered card.</summary>
    public string Text { get; private set; } = "";

    public string Poet { get; private set; } = "";

    /// <summary>Occasion this verse suits. Empty means it suits any.</summary>
    public string OccasionSlug { get; private set; } = "";

    public int SortOrder { get; private set; }

    public bool IsActive { get; private set; }
}

public sealed class TemplatesDbContext(DbContextOptions<TemplatesDbContext> options) : DbContext(options)
{
    public DbSet<CardTemplate> Templates => Set<CardTemplate>();
    public DbSet<Occasion> Occasions => Set<Occasion>();
    public DbSet<Poem> Poems => Set<Poem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("templates");
        var t = modelBuilder.Entity<CardTemplate>();
        t.HasKey(x => x.Id);
        t.Property(x => x.Slug).HasMaxLength(60).IsRequired();
        t.HasIndex(x => x.Slug).IsUnique();
        t.Property(x => x.Name).HasMaxLength(120).IsRequired();
        t.Property(x => x.Category).HasMaxLength(40).IsRequired();
        t.HasIndex(x => x.Category);
        t.Property(x => x.Family).HasMaxLength(40).IsRequired();
        t.Property(x => x.Accent).HasMaxLength(7).IsRequired();
        t.Property(x => x.Background).HasMaxLength(240).IsRequired();
        t.Property(x => x.Ink).HasMaxLength(7).IsRequired();
        t.Property(x => x.Frame).HasMaxLength(20).IsRequired();
        t.Property(x => x.Ornament).HasMaxLength(20).IsRequired();
        t.Property(x => x.Typeface).HasMaxLength(20).IsRequired();
        t.Property(x => x.Layout).HasMaxLength(20).IsRequired();
        t.Property(x => x.Artwork).HasMaxLength(60).IsRequired();
        t.Property(x => x.SafeTop).HasDefaultValue(30);
        t.Property(x => x.SafeBottom).HasDefaultValue(75);
        // nvarchar(max) with an empty default: a layout is read and written whole, never queried
        // by element, so there is nothing here worth a length limit or an index. The element cap
        // that actually protects the renderer lives in CardLayout, not in the column.
        t.Property(x => x.Elements).HasDefaultValue("").IsRequired();

        var o = modelBuilder.Entity<Occasion>();
        o.HasKey(x => x.Id);
        o.Property(x => x.Slug).HasMaxLength(40).IsRequired();
        o.HasIndex(x => x.Slug).IsUnique();
        o.Property(x => x.Title).HasMaxLength(60).IsRequired();
        o.Property(x => x.Tagline).HasMaxLength(120).IsRequired();
        o.Property(x => x.Icon).HasMaxLength(40).IsRequired();
        o.Property(x => x.PrimaryLabel).HasMaxLength(60).IsRequired();
        o.Property(x => x.SecondaryLabel).HasMaxLength(60).IsRequired();
        o.Property(x => x.Joiner).HasMaxLength(10).IsRequired();
        o.Property(x => x.DefaultKicker).HasMaxLength(120).IsRequired();
        o.Property(x => x.DefaultMessage).HasMaxLength(400).IsRequired();
        o.HasIndex(x => new { x.IsActive, x.SortOrder });

        // The six wedding-side occasions ship active. The rest are seeded but hidden: an
        // occasion with no templates behind it would show the visitor an empty gallery, so
        // each one is switched on once its artwork exists.
        o.HasData(
            new
            {
                Id = Guid.Parse("b7c41f20-0000-4000-9000-000000000001"),
                Slug = "wedding",
                Title = "عروسی",
                Tagline = "دعوت‌نامه جشن عروسی",
                Icon = "wedding",
                SortOrder = 1,
                IsActive = true,
                PrimaryLabel = "نام عروس",
                SecondaryLabel = "نام داماد",
                Joiner = "و",
                DefaultKicker = "آغاز یک زندگی، کنار هم",
                DefaultMessage = "با حضور شما شادی ما کامل می‌شود",
            },
            new
            {
                Id = Guid.Parse("b7c41f20-0000-4000-9000-000000000002"),
                Slug = "aghd",
                Title = "عقد",
                Tagline = "دعوت به مراسم عقد",
                Icon = "aghd",
                SortOrder = 2,
                IsActive = true,
                PrimaryLabel = "نام عروس",
                SecondaryLabel = "نام داماد",
                Joiner = "و",
                DefaultKicker = "پیوند دو دل",
                DefaultMessage = "با حضور شما شادی ما کامل می‌شود",
            },
            new
            {
                Id = Guid.Parse("b7c41f20-0000-4000-9000-000000000003"),
                Slug = "engagement",
                Title = "نامزدی",
                Tagline = "جشن نامزدی و حلقه",
                Icon = "engagement",
                SortOrder = 3,
                IsActive = true,
                PrimaryLabel = "نام عروس",
                SecondaryLabel = "نام داماد",
                Joiner = "و",
                DefaultKicker = "آغاز یک قرار",
                DefaultMessage = "به جشن نامزدی ما خوش آمدید",
            },
            new
            {
                Id = Guid.Parse("b7c41f20-0000-4000-9000-000000000004"),
                Slug = "baleboron",
                Title = "بله‌برون",
                Tagline = "مراسم بله‌برون",
                Icon = "baleboron",
                SortOrder = 4,
                IsActive = true,
                PrimaryLabel = "نام عروس",
                SecondaryLabel = "نام داماد",
                Joiner = "و",
                DefaultKicker = "یک بله، یک آغاز",
                DefaultMessage = "در این روز خوش کنار ما باشید",
            },
            new
            {
                Id = Guid.Parse("b7c41f20-0000-4000-9000-000000000005"),
                Slug = "hanabandan",
                Title = "حنابندان",
                Tagline = "شب حنابندان",
                Icon = "hanabandan",
                SortOrder = 5,
                IsActive = true,
                PrimaryLabel = "نام عروس",
                SecondaryLabel = "نام داماد",
                Joiner = "و",
                DefaultKicker = "شب حنا، شب شادی",
                DefaultMessage = "در شب حنابندان منتظر شما هستیم",
            },
            new
            {
                Id = Guid.Parse("b7c41f20-0000-4000-9000-000000000006"),
                Slug = "anniversary",
                Title = "سالگرد ازدواج",
                Tagline = "سالگرد و تجدید پیمان",
                Icon = "anniversary",
                SortOrder = 6,
                IsActive = true,
                PrimaryLabel = "نام همسر اول",
                SecondaryLabel = "نام همسر دوم",
                Joiner = "و",
                DefaultKicker = "سال‌هایی که گذشت",
                DefaultMessage = "در جشن سالگرد ما شریک باشید",
            },
            new
            {
                Id = Guid.Parse("b7c41f20-0000-4000-9000-000000000007"),
                Slug = "memorial",
                Title = "مجلس ترحیم",
                Tagline = "یادبود و مراسم ترحیم",
                Icon = "memorial",
                SortOrder = 7,
                IsActive = false,
                PrimaryLabel = "نام درگذشته",
                SecondaryLabel = "",
                Joiner = "",
                DefaultKicker = "یاد او همیشه با ماست",
                DefaultMessage = "به مراسم یادبود ایشان دعوت می‌شوید",
            },
            new
            {
                Id = Guid.Parse("b7c41f20-0000-4000-9000-000000000008"),
                Slug = "birthday",
                Title = "تولد",
                Tagline = "جشن تولد",
                Icon = "birthday",
                SortOrder = 8,
                IsActive = false,
                PrimaryLabel = "نام صاحب جشن",
                SecondaryLabel = "",
                Joiner = "",
                DefaultKicker = "یک سال تازه",
                DefaultMessage = "به جشن تولد دعوتید",
            },
            new
            {
                Id = Guid.Parse("b7c41f20-0000-4000-9000-000000000009"),
                Slug = "fathers-day",
                Title = "روز پدر",
                Tagline = "تبریک روز پدر",
                Icon = "fathers-day",
                SortOrder = 9,
                IsActive = false,
                PrimaryLabel = "نام پدر",
                SecondaryLabel = "",
                Joiner = "",
                DefaultKicker = "برای پدر",
                DefaultMessage = "روزت مبارک",
            },
            new
            {
                Id = Guid.Parse("b7c41f20-0000-4000-9000-000000000010"),
                Slug = "mothers-day",
                Title = "روز مادر",
                Tagline = "تبریک روز مادر",
                Icon = "mothers-day",
                SortOrder = 10,
                IsActive = false,
                PrimaryLabel = "نام مادر",
                SecondaryLabel = "",
                Joiner = "",
                DefaultKicker = "برای مادر",
                DefaultMessage = "روزت مبارک",
            },
            new
            {
                Id = Guid.Parse("b7c41f20-0000-4000-9000-000000000011"),
                Slug = "students-day",
                Title = "روز دانشجو",
                Tagline = "گرامیداشت روز دانشجو",
                Icon = "students-day",
                SortOrder = 11,
                IsActive = false,
                PrimaryLabel = "نام مناسبت",
                SecondaryLabel = "",
                Joiner = "",
                DefaultKicker = "روز دانشجو",
                DefaultMessage = "گرامی باد",
            });

        // The first two ids are the ones shipped in the starter. They are kept so existing
        // drafts still resolve to a real template; the rest are new rows.
        t.HasData(
            // --- minimal ------------------------------------------------------------------
            new
            {
                Id = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b0103"),
                Slug = "sepid-sade",
                Name = "سپید ساده",
                Version = 2,
                Category = "wedding",
                Family = "minimal",
                Accent = "#847b57",
                Background = "linear-gradient(180deg,#fffdf9 0%,#f6f1e8 100%)",
                Ink = "#3c3a36",
                Frame = "none",
                Ornament = "rule",
                Typeface = "serif",
                Layout = "centered",
                Artwork = "sepid-sade",
                SafeTop = 44,
                SafeBottom = 80,
                Elements = CardLayoutDefaults.Json(44, 80),
            },
            new
            {
                Id = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b0104"),
                Slug = "khat-e-noor",
                Name = "خط نور",
                Version = 2,
                Category = "aghd",
                Family = "minimal",
                Accent = "#7c764f",
                Background = "#fbfaf7",
                Ink = "#3c3a36",
                Frame = "none",
                Ornament = "dot",
                Typeface = "display",
                Layout = "centered",
                Artwork = "khat-e-noor",
                SafeTop = 22,
                SafeBottom = 58,
                Elements = CardLayoutDefaults.Json(22, 58),
            },
            new
            {
                Id = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b0105"),
                Slug = "sadegi-talaei",
                Name = "سادگی طلایی",
                Version = 2,
                Category = "anniversary",
                Family = "minimal",
                Accent = "#807547",
                Background = "linear-gradient(180deg,#fdfbf6 0%,#f4ecdd 100%)",
                Ink = "#3c3a36",
                Frame = "none",
                Ornament = "rule",
                Typeface = "display",
                Layout = "centered",
                Artwork = "sadegi-talaei",
                SafeTop = 22,
                SafeBottom = 58,
                Elements = CardLayoutDefaults.Json(22, 58),
            },

            // --- watercolor ---------------------------------------------------------------
            new
            {
                Id = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b0106"),
                Slug = "golab-abrang",
                Name = "گلاب آبرنگ",
                Version = 1,
                Category = "wedding",
                Family = "watercolor",
                Accent = "#c17f8f",
                Background = "radial-gradient(120% 80% at 50% 0%,#fdeef1 0%,#fbf7f4 55%,#f7eee9 100%)",
                Ink = "#4a3a3e",
                Frame = "none",
                Ornament = "floral",
                Typeface = "serif",
                Layout = "centered",
                Artwork = "golab-abrang",
                SafeTop = 37,
                SafeBottom = 67,
                Elements = CardLayoutDefaults.Json(37, 67),
            },
            new
            {
                Id = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b0107"),
                Slug = "shaghayegh",
                Name = "شقایق",
                Version = 2,
                Category = "engagement",
                Family = "watercolor",
                Accent = "#85785d",
                Background = "radial-gradient(100% 70% at 20% 10%,#fdeceb 0%,#fcf8f5 60%,#f8f1ec 100%)",
                Ink = "#3c3a36",
                Frame = "none",
                Ornament = "floral",
                Typeface = "serif",
                Layout = "centered",
                Artwork = "shaghayegh",
                SafeTop = 40,
                SafeBottom = 76,
                Elements = CardLayoutDefaults.Json(40, 76),
            },
            new
            {
                Id = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b0108"),
                Slug = "banafsheh",
                Name = "بنفشه",
                Version = 2,
                Category = "baleboron",
                Family = "watercolor",
                Accent = "#6d5c7f",
                Background = "radial-gradient(110% 80% at 80% 0%,#f1edfa 0%,#faf8fc 55%,#f4f1f7 100%)",
                Ink = "#3c3a36",
                Frame = "none",
                Ornament = "floral",
                Typeface = "serif",
                Layout = "centered",
                Artwork = "banafsheh",
                SafeTop = 45,
                SafeBottom = 81,
                Elements = CardLayoutDefaults.Json(45, 81),
            },
            new
            {
                Id = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b0109"),
                Slug = "hana-o-gol",
                Name = "حنا و گل",
                Version = 2,
                Category = "hanabandan",
                Family = "watercolor",
                Accent = "#80673d",
                Background = "linear-gradient(160deg,#fdf4e4 0%,#fbf6ee 55%,#f6ead6 100%)",
                Ink = "#3c3a36",
                Frame = "none",
                Ornament = "floral",
                Typeface = "serif",
                Layout = "centered",
                Artwork = "hana-o-gol",
                SafeTop = 14,
                SafeBottom = 50,
                Elements = CardLayoutDefaults.Json(14, 50),
            },

            // --- persian classic ----------------------------------------------------------
            new
            {
                Id = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b0101"),
                Slug = "bagh-e-irani",
                Name = "باغ ایرانی",
                Version = 2,
                Category = "wedding",
                Family = "persian",
                Accent = "#918063",
                Background = "linear-gradient(180deg,#f4f7f3 0%,#e9f0ea 100%)",
                Ink = "#3c3a36",
                Frame = "none",
                Ornament = "paisley",
                Typeface = "serif",
                Layout = "centered",
                Artwork = "bagh-e-irani",
                SafeTop = 31,
                SafeBottom = 67,
                Elements = CardLayoutDefaults.Json(31, 67),
            },
            new
            {
                Id = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b010b"),
                Slug = "kashi-firouzeh",
                Name = "کاشی فیروزه",
                Version = 2,
                Category = "hanabandan",
                Family = "persian",
                Accent = "#566c84",
                Background = "linear-gradient(180deg,#eff7f8 0%,#e2eff1 100%)",
                Ink = "#3c3a36",
                Frame = "none",
                Ornament = "geometric",
                Typeface = "display",
                Layout = "centered",
                Artwork = "kashi-firouzeh",
                SafeTop = 34,
                SafeBottom = 70,
                Elements = CardLayoutDefaults.Json(34, 70),
            },

            // --- photo-led ----------------------------------------------------------------
            new
            {
                Id = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b0102"),
                Slug = "roz-o-morvarid",
                Name = "رز و مروارید",
                Version = 2,
                Category = "wedding",
                Family = "photo",
                Accent = "#8a7c5c",
                Background = "linear-gradient(180deg,#fbf4f6 0%,#f2e6ea 100%)",
                Ink = "#3c3a36",
                Frame = "none",
                Ornament = "dot",
                Typeface = "serif",
                Layout = "centered",
                Artwork = "roz-o-morvarid",
                SafeTop = 26,
                SafeBottom = 62,
                Elements = CardLayoutDefaults.Json(26, 62),
            },
            new
            {
                Id = Guid.Parse("a41a6319-3438-4ef9-89da-fae6b30b010c"),
                Slug = "ghab-e-khatereh",
                Name = "قاب خاطره",
                Version = 2,
                Category = "anniversary",
                Family = "photo",
                Accent = "#7d6143",
                Background = "linear-gradient(180deg,#f6f7f9 0%,#e9ebef 100%)",
                Ink = "#3c3a36",
                Frame = "none",
                Ornament = "rule",
                Typeface = "serif",
                Layout = "centered",
                Artwork = "ghab-e-khatereh",
                SafeTop = 33,
                SafeBottom = 69,
                Elements = CardLayoutDefaults.Json(33, 69),
            });

        var p = modelBuilder.Entity<Poem>();
        p.HasKey(x => x.Id);
        p.Property(x => x.Text).HasMaxLength(400).IsRequired();
        p.Property(x => x.Poet).HasMaxLength(60).IsRequired();
        p.Property(x => x.OccasionSlug).HasMaxLength(40).IsRequired();
        p.HasIndex(x => new { x.IsActive, x.OccasionSlug, x.SortOrder });

        // A deliberately short starting list. Every line here is classical and out of copyright,
        // and every attribution was checked. Extending it is an admin job, not a deployment.
        p.HasData(
            new
            {
                Id = Guid.Parse("c3d52a10-0000-4000-9000-000000000001"),
                Text = "درخت دوستی بنشان که کام دل به بار آرد\nنهال دشمنی برکن که رنج بی‌شمار آرد",
                Poet = "حافظ",
                OccasionSlug = "",
                SortOrder = 1,
                IsActive = true,
            },
            new
            {
                Id = Guid.Parse("c3d52a10-0000-4000-9000-000000000002"),
                Text = "دست از طلب ندارم تا کام من برآید\nیا تن رسد به جانان یا جان ز تن برآید",
                Poet = "حافظ",
                OccasionSlug = "",
                SortOrder = 2,
                IsActive = true,
            },
            new
            {
                Id = Guid.Parse("c3d52a10-0000-4000-9000-000000000003"),
                Text = "مرا مهر سیه‌چشمان ز سر بیرون نخواهد شد\nقضای آسمان است این و دیگرگون نخواهد شد",
                Poet = "حافظ",
                OccasionSlug = "",
                SortOrder = 3,
                IsActive = true,
            },
            new
            {
                Id = Guid.Parse("c3d52a10-0000-4000-9000-000000000004"),
                Text = "به جهان خرم از آنم که جهان خرم از اوست\nعاشقم بر همه عالم که همه عالم از اوست",
                Poet = "سعدی",
                OccasionSlug = "",
                SortOrder = 4,
                IsActive = true,
            },
            new
            {
                Id = Guid.Parse("c3d52a10-0000-4000-9000-000000000005"),
                Text = "بنی‌آدم اعضای یک پیکرند\nکه در آفرینش ز یک گوهرند",
                Poet = "سعدی",
                OccasionSlug = "",
                SortOrder = 5,
                IsActive = true,
            },
            new
            {
                Id = Guid.Parse("c3d52a10-0000-4000-9000-000000000006"),
                Text = "عشق آن شعله است کاو چون برفروخت\nهر چه جز معشوق باقی جمله سوخت",
                Poet = "مولوی",
                OccasionSlug = "",
                SortOrder = 6,
                IsActive = true,
            },
            new
            {
                Id = Guid.Parse("c3d52a10-0000-4000-9000-000000000007"),
                Text = "به نام آنکه جان را فکرت آموخت\nچراغ دل به نور جان برافروخت",
                Poet = "نظامی",
                OccasionSlug = "",
                SortOrder = 7,
                IsActive = true,
            });
    }
}
