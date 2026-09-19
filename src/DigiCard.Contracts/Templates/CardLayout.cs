using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DigiCard.Contracts.Templates;

/// <summary>
/// What an element on a card is for. The role decides where its content comes from, which CSS
/// class it gets and which design variants are offered for it - it is never interpolated into a
/// style attribute.
///
/// Bound roles (kicker, names, message, date, time, address, photo) draw their content from the
/// card's own data, so an occasion's labels and a draft's names flow into the layout without the
/// layout storing a copy. Free roles (poem, text) carry their own content.
/// </summary>
public static class CardRole
{
    public const string Kicker = "kicker";
    public const string Names = "names";

    /// <summary>
    /// The two names as separate boxes, with the joining word between them. A card often wants
    /// one name raised above the other, or set larger, and a single combined box cannot do that.
    /// </summary>
    public const string Name1 = "name1";
    public const string Name2 = "name2";
    public const string Joiner = "joiner";
    public const string Divider = "divider";
    public const string Message = "message";
    public const string Poem = "poem";
    public const string Date = "date";
    public const string Time = "time";
    public const string Address = "address";
    public const string Photo = "photo";
    public const string Text = "text";

    private static readonly HashSet<string> All =
        [Kicker, Names, Name1, Name2, Joiner, Divider, Message, Poem, Date, Time, Address, Photo, Text];

    /// <summary>The three roles a split names block turns into.</summary>
    public static readonly string[] SplitNameRoles = [Name1, Joiner, Name2];

    /// <summary>Roles whose text lives in the element rather than in the card's data.</summary>
    private static readonly HashSet<string> Free = [Poem, Text];

    /// <summary>
    /// The design variants each role offers, first entry being its default. A variant is a
    /// presentation choice inside one role - a date shown in a ring rather than on a line - so
    /// keeping them per role stops the editor offering "circle" for a paragraph of text.
    /// </summary>
    private static readonly Dictionary<string, string[]> Variants = new(StringComparer.Ordinal)
    {
        [Date] = ["plain", "weekday", "numeric", "circle", "boxed", "stacked"],
        [Time] = ["plain", "boxed", "circle"],
        [Address] = ["plain", "pin", "boxed"],
        [Divider] = ["rule", "dots", "floral"],
        [Photo] = ["rect", "rounded", "circle", "oval", "arch"],
    };

    public static bool IsKnown(string? role) => role is not null && All.Contains(role);

    public static bool CarriesOwnText(string role) => Free.Contains(role);

    /// <summary>The variants offered for a role, or empty when the role has none.</summary>
    public static IReadOnlyList<string> VariantsFor(string role) =>
        Variants.TryGetValue(role, out var list) ? list : [];

    /// <summary>Falls back to the role's first variant, or "plain" for roles that have none.</summary>
    public static string NormalizeVariant(string role, string? variant)
    {
        if (!Variants.TryGetValue(role, out var list)) return "plain";
        return variant is not null && Array.IndexOf(list, variant) >= 0 ? variant : list[0];
    }
}

/// <summary>
/// Everything about an element's appearance, as tokens, strictly patterned strings and clamped
/// numbers.
///
/// This is the security boundary for the editor. A card layout is written by users and CardPreview
/// interpolates these values into a style attribute, so nothing here may be free text. Note how
/// the free colour works: it is not filtered for dangerous characters, it has to match #rrggbb
/// exactly. A whitelist pattern is the only kind of validation worth trusting here - a blocklist
/// would eventually miss something, and "#fff;background:url(...)" would be that something.
/// </summary>
public sealed partial record CardElementStyle
{
    public static readonly CardElementStyle Default = new();

    /// <summary>Type size as a percentage of card width, so it scales with the card.</summary>
    public double FontSize { get; init; } = 4;

    /// <summary>body | display | nastaliq</summary>
    public string Font { get; init; } = "body";

    /// <summary>normal | medium | bold</summary>
    public string Weight { get; init; } = "normal";

    /// <summary>right | center | left</summary>
    public string Align { get; init; } = "center";

    /// <summary>
    /// Either one of the card's own palette tokens (ink | accent | muted | paper) or an exact
    /// #rrggbb. Tokens track the template's colours; a literal is the user's own choice.
    /// </summary>
    public string Color { get; init; } = "ink";

    public double LineHeight { get; init; } = 1.7;

    public double Opacity { get; init; } = 1;

    /// <summary>The role's design variant - see CardRole.VariantsFor.</summary>
    public string Variant { get; init; } = "plain";

    /// <summary>Digit shapes for dates and times: fa | en.</summary>
    public string Digits { get; init; } = "fa";

    /// <summary>none | thin | double | gold | art. "art" uses <see cref="FrameArt"/>.</summary>
    public string Frame { get; init; } = "none";

    /// <summary>
    /// Slug of a painted frame under wwwroot/card-art/frames. Rendered as a background image, so a
    /// slug with no file behind it shows nothing rather than a broken-image icon.
    /// </summary>
    public string? FrameArt { get; init; }

    /// <summary>How far the edges of a photo fade out, 0 (hard edge) to 100 (fully soft).</summary>
    public double Feather { get; init; }

    /// <summary>
    /// cover | contain. Cover fills the frame and crops whatever does not fit; contain shows the
    /// whole picture and leaves space around it. Cover is the default because a frame someone
    /// shaped on purpose should stay that shape - but it is the setting that cuts heads off a
    /// portrait dropped into a square, so it has to be one click to change.
    /// </summary>
    public string Fit { get; init; } = "cover";

    /// <summary>
    /// Which part of the picture stays in view when the frame crops it, as a percentage across
    /// (0 = the right edge in a right-to-left sense of the word only by accident - this is the CSS
    /// axis, 0 is the left edge of the image) and down (0 = the top). A portrait photo dropped
    /// into a circle or a wide rectangle loses most of its height, and the face is almost never
    /// in the middle of what is left, so this is the difference between a usable card and a
    /// picture of someone's chin.
    /// </summary>
    public double FocusX { get; init; } = 50;

    public double FocusY { get; init; } = 50;

    /// <summary>
    /// How far in the picture is scaled inside its frame, as a percentage. 100 is however the fit
    /// left it; above that the photo is pushed in closer, below it pulls back and shows more of
    /// the picture than the frame's shape would otherwise allow, leaving the card's own colour at
    /// the sides. The zoom happens around the focus point, so moving in does not undo the
    /// positioning the user just did.
    /// </summary>
    public double Zoom { get; init; } = 100;

    private static readonly HashSet<string> Fonts = ["body", "display", "nastaliq"];
    private static readonly HashSet<string> Weights = ["normal", "medium", "bold"];
    private static readonly HashSet<string> Aligns = ["right", "center", "left"];
    private static readonly HashSet<string> ColorTokens = ["ink", "accent", "muted", "paper"];
    private static readonly HashSet<string> Frames = ["none", "thin", "double", "gold", "art"];
    private static readonly HashSet<string> Fits = ["cover", "contain"];

    [GeneratedRegex("^#[0-9a-fA-F]{6}$")]
    private static partial Regex HexColor();

    [GeneratedRegex("^[a-z0-9-]{1,40}$")]
    private static partial Regex Slug();

    /// <summary>True when the colour is a literal rather than one of the card's palette tokens.</summary>
    public bool HasLiteralColor => Color is not null && !ColorTokens.Contains(Color);

    public CardElementStyle Normalize(string role) => new()
    {
        FontSize = CardLayout.Clamp(FontSize, 1, 30),
        Font = Pick(Fonts, Font, "body"),
        Weight = Pick(Weights, Weight, "normal"),
        Align = Pick(Aligns, Align, "center"),
        Color = NormalizeColor(Color),
        LineHeight = CardLayout.Clamp(LineHeight, 0.8, 4),
        Opacity = CardLayout.Clamp(Opacity, 0.05, 1),
        Variant = CardRole.NormalizeVariant(role, Variant),
        Digits = Digits == "en" ? "en" : "fa",
        Frame = Pick(Frames, Frame, "none"),
        FrameArt = FrameArt is not null && Slug().IsMatch(FrameArt) ? FrameArt : null,
        Feather = CardLayout.Clamp(Feather, 0, 100),
        Fit = Pick(Fits, Fit, "cover"),
        FocusX = CardLayout.Clamp(FocusX, 0, 100),
        FocusY = CardLayout.Clamp(FocusY, 0, 100),
        // A photo at four times its size is already unrecognisable, and a quarter is a stamp in
        // the middle of the frame. Both ends are past useful; neither can break the layout.
        Zoom = CardLayout.Clamp(Zoom, 25, 400),
    };

    // Every token field goes through here. JSON can carry an explicit null for any of them, and
    // HashSet.Contains(null) throws, so the null check is not decoration.
    private static string Pick(HashSet<string> allowed, string? value, string fallback) =>
        value is not null && allowed.Contains(value) ? value : fallback;

    private static string NormalizeColor(string? value) =>
        value is not null && (ColorTokens.Contains(value) || HexColor().IsMatch(value)) ? value : "ink";
}

/// <summary>
/// One positioned element. Coordinates are percentages of the card, never pixels: the same layout
/// has to hold on a 320px phone and in a 2000px export, and the editor drags in percentages for
/// exactly that reason.
///
/// X and Y are the box's top-left corner and are physical, not writing-direction relative. The
/// card is right-to-left, but "left" here always means the left edge of the card, because the
/// editor moves things with a mouse and a mouse has no writing direction.
/// </summary>
public sealed partial record CardElement
{
    /// <summary>Stable within one layout. The editor uses it for selection, undo and re-ordering.</summary>
    public string Id { get; init; } = "";

    public string Role { get; init; } = CardRole.Text;

    /// <summary>Left edge, as a percentage of card width.</summary>
    public double X { get; init; }

    /// <summary>Top edge, as a percentage of card height.</summary>
    public double Y { get; init; }

    /// <summary>Width, as a percentage of card width.</summary>
    public double W { get; init; } = 80;

    /// <summary>
    /// Height as a percentage of card height, or null to let the content decide. Text is normally
    /// null - a four-line poem needs more room than a name, and a fixed box would clip it. A photo
    /// always has one, because an empty frame has no content to be sized by.
    /// </summary>
    public double? H { get; init; }

    /// <summary>Degrees, clockwise, about the element's own centre.</summary>
    public double Rotation { get; init; }

    /// <summary>Stacking order. Higher draws later.</summary>
    public int Z { get; init; }

    /// <summary>Authored text, for roles that carry their own content. Ignored for the others.</summary>
    public string? Text { get; init; }

    /// <summary>Attribution for a poem: "حافظ". Free roles only.</summary>
    public string? Caption { get; init; }

    /// <summary>Asset path for an image element, relative to wwwroot.</summary>
    public string? Source { get; init; }

    public CardElementStyle Style { get; init; } = CardElementStyle.Default;

    public CardElement Normalize(int index) => Normalize(index, CardRole.IsKnown(Role) ? Role : CardRole.Text);

    private CardElement Normalize(int index, string role) => new()
    {
        Id = string.IsNullOrWhiteSpace(Id) ? $"e{index}" : Id.Trim()[..Math.Min(Id.Trim().Length, 40)],
        Role = role,
        // Positions are allowed slightly outside the card: a painted ornament often bleeds past
        // the edge on purpose. They are not allowed far enough out to vanish.
        X = CardLayout.Clamp(X, -25, 120),
        Y = CardLayout.Clamp(Y, -25, 120),
        W = CardLayout.Clamp(W, 2, 150),
        // A photo with no height would collapse to nothing, since it has no text to size it.
        H = H is null
            ? (role == CardRole.Photo ? 30 : null)
            : CardLayout.Clamp(H.Value, 1, 150),
        Rotation = CardLayout.Clamp(Rotation, -180, 180),
        Z = (int)CardLayout.Clamp(Z, 0, 999),
        // Typed text is kept exactly as typed. Trimming it here looked harmless and was not:
        // every keystroke in the editor runs through this method, so a trailing space was removed
        // the instant it was typed, the textarea's value no longer matched what the model held,
        // Blazor reset the DOM node, and the caret jumped to the start. That is what "I cannot
        // write anything in a text box" was. Length is still capped; whitespace is the user's.
        Text = Cap(Text, 600),
        Caption = Cap(Caption, 80),
        // Not typed by hand - an asset path, so trimming it is safe and useful.
        // Anything that is not one of our own media ids is dropped rather than cleaned up:
        // there is no partially-acceptable URL here.
        Source = MediaSource().IsMatch(Source ?? "") ? Source : null,
        // Pattern rather than ?? : Style is non-nullable in the contract, but JSON can still
        // carry an explicit null, and that must not reach the renderer.
        Style = (Style is { } style ? style : CardElementStyle.Default).Normalize(role),
    };

    /// <summary>
    /// A card may only point at an image this site stores. Source ends up in an img src, so
    /// without this an element could be made to reference any URL on the internet: a card whose
    /// picture silently changes, or stops loading, because someone else's server decided so - and
    /// bytes we neither sized, converted nor cached, on the one page where load time matters most.
    /// </summary>
    [GeneratedRegex("^/api/media/[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$")]
    private static partial Regex MediaSource();

    private static string? Trim(string? value, int max)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var text = value.Trim();
        return text.Length <= max ? text : text[..max];
    }

    /// <summary>Caps length without touching anything the user typed, spaces included.</summary>
    private static string? Cap(string? value, int max)
    {
        if (value is null || value.Length == 0) return null;
        return value.Length <= max ? value : value[..max];
    }
}

/// <summary>
/// A card as a list of elements. This replaces the fixed kicker/names/message markup: the editor
/// needs something it can move things around in, and a template is now a saved default layout
/// rather than a hardcoded arrangement.
///
/// Stored as JSON in a single column. That is a deliberate trade: a layout is only ever read and
/// written whole, never queried by element, so a child table would buy nothing and cost a join.
/// </summary>
public sealed record CardLayout
{
    /// <summary>A hostile or broken layout must not be able to render ten thousand boxes.</summary>
    public const int MaxElements = 40;

    public static readonly CardLayout Empty = new([]);

    public CardLayout(IReadOnlyList<CardElement> elements) => Elements = elements;

    public IReadOnlyList<CardElement> Elements { get; }

    public bool IsEmpty => Elements.Count == 0;

    private static readonly JsonSerializerOptions Json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    /// <summary>
    /// Reads a stored layout. Returns false rather than throwing: a single malformed row must
    /// degrade one card to its fallback markup, not take down the gallery. Callers that have a
    /// logger should say something when this returns false.
    /// </summary>
    public static bool TryParse(string? json, out CardLayout layout)
    {
        layout = Empty;
        if (string.IsNullOrWhiteSpace(json)) return true;

        try
        {
            var raw = JsonSerializer.Deserialize<List<CardElement>>(json, Json);
            if (raw is null) return false;

            layout = FromElements(raw);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    /// <summary>Normalizes, de-duplicates ids and caps the count. The only way to build a layout.</summary>
    public static CardLayout FromElements(IEnumerable<CardElement> elements)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var kept = new List<CardElement>();

        foreach (var element in elements.Take(MaxElements))
        {
            var normalized = element.Normalize(kept.Count);

            // Duplicate ids would make the editor's selection ambiguous, so the later one is
            // renamed rather than dropped - losing a user's element silently is worse.
            var id = normalized.Id;
            for (var suffix = 2; !seen.Add(id); suffix++)
                id = $"{normalized.Id}-{suffix}";

            kept.Add(normalized with { Id = id });
        }

        return new CardLayout(kept);
    }

    public string Serialize() => JsonSerializer.Serialize(Elements, Json);

    /// <summary>
    /// Formats a number for a CSS declaration. Always invariant: under fa-IR a plain ToString()
    /// yields Persian digits and a different decimal separator, which produces a style attribute
    /// the browser silently discards. This is the only place layout numbers become text.
    /// </summary>
    public static string Css(double value) =>
        Math.Round(value, 3).ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// The card's shape, width over height, as the stylesheet draws it. Needed whenever a size in
    /// one axis has to be turned into a percentage of the other - which is what makes a photo's
    /// frame match the photo.
    /// </summary>
    public const double CardAspect = 2.0 / 3.0;

    internal static double Clamp(double value, double min, double max) =>
        double.IsNaN(value) ? min : value < min ? min : value > max ? max : value;
}
