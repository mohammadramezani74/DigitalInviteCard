using System.Globalization;
using DigiCard.Contracts.Templates;
using Xunit;

namespace DigiCard.Tests;

/// <summary>
/// A card layout is the one piece of the model that will shortly be written by users and then
/// interpolated into a style attribute. These tests are about that boundary: what survives
/// normalization, what does not, and that the numbers reach CSS in a form a browser accepts.
/// </summary>
public sealed class CardLayoutTests
{
    [Fact]
    public void Round_trips_through_json()
    {
        var original = CardLayoutDefaults.For(30, 70);

        Assert.True(CardLayout.TryParse(original.Serialize(), out var parsed));
        Assert.Equal(original.Elements.Count, parsed.Elements.Count);

        foreach (var (a, b) in original.Elements.Zip(parsed.Elements))
        {
            Assert.Equal(a.Id, b.Id);
            Assert.Equal(a.Role, b.Role);
            Assert.Equal(a.X, b.X, 3);
            Assert.Equal(a.Y, b.Y, 3);
            Assert.Equal(a.Style.FontSize, b.Style.FontSize, 3);
            Assert.Equal(a.Style.Font, b.Style.Font);
        }
    }

    [Fact]
    public void Seed_json_is_stable_across_calls()
    {
        // EF seed data is compared as literals on every model build. If this drifts, every
        // startup would look like a pending model change.
        Assert.Equal(CardLayoutDefaults.Json(44, 80), CardLayoutDefaults.Json(44, 80));
    }

    [Fact]
    public void Empty_and_missing_layouts_are_empty_not_errors()
    {
        Assert.True(CardLayout.TryParse(null, out var fromNull));
        Assert.True(fromNull.IsEmpty);

        Assert.True(CardLayout.TryParse("   ", out var fromBlank));
        Assert.True(fromBlank.IsEmpty);
    }

    [Fact]
    public void Malformed_json_is_reported_rather_than_thrown()
    {
        // The home page renders cards and has to survive one bad row in the catalog.
        Assert.False(CardLayout.TryParse("{ not json", out var layout));
        Assert.True(layout.IsEmpty);
    }

    [Fact]
    public void Unknown_tokens_fall_back_instead_of_reaching_css()
    {
        const string hostile = """
            [{"id":"x","role":"drop-tables","x":10,"y":10,"w":50,
              "style":{"font":"'; background:url(evil)","weight":"9999","align":"middle",
                       "color":"#fff;position:fixed","fontSize":9999,"opacity":50,"lineHeight":-3}}]
            """;

        Assert.True(CardLayout.TryParse(hostile, out var layout));
        var element = Assert.Single(layout.Elements);

        Assert.Equal(CardRole.Text, element.Role);
        Assert.Equal("body", element.Style.Font);
        Assert.Equal("normal", element.Style.Weight);
        Assert.Equal("center", element.Style.Align);
        Assert.Equal("ink", element.Style.Color);
        Assert.Equal(30d, element.Style.FontSize);
        Assert.Equal(1d, element.Style.Opacity);
        Assert.Equal(0.8, element.Style.LineHeight, 3);
    }

    [Fact]
    public void Positions_are_clamped_to_a_visible_range()
    {
        const string offscreen = """[{"id":"a","role":"text","x":-9000,"y":9000,"w":0,"rotation":720,"z":-5}]""";

        Assert.True(CardLayout.TryParse(offscreen, out var layout));
        var element = Assert.Single(layout.Elements);

        Assert.Equal(-25d, element.X);
        Assert.Equal(120d, element.Y);
        Assert.Equal(2d, element.W);
        Assert.Equal(180d, element.Rotation);
        Assert.Equal(0, element.Z);
    }

    [Fact]
    public void Element_count_is_capped()
    {
        var many = Enumerable.Range(0, CardLayout.MaxElements * 3)
            .Select(i => new CardElement { Id = $"e{i}", Role = CardRole.Text, Text = "x" });

        Assert.Equal(CardLayout.MaxElements, CardLayout.FromElements(many).Elements.Count);
    }

    [Fact]
    public void Duplicate_ids_are_renamed_never_dropped()
    {
        var layout = CardLayout.FromElements(
        [
            new CardElement { Id = "names", Role = CardRole.Names },
            new CardElement { Id = "names", Role = CardRole.Text, Text = "دوم" },
            new CardElement { Id = "names", Role = CardRole.Text, Text = "سوم" },
        ]);

        Assert.Equal(3, layout.Elements.Count);
        Assert.Equal(new[] { "names", "names-2", "names-3" }, layout.Elements.Select(e => e.Id));
    }

    [Fact]
    public void Numbers_reach_css_invariantly_under_a_persian_culture()
    {
        // Under fa-IR a plain ToString() yields Persian digits and a different decimal separator,
        // and the browser discards the whole declaration. This is the bug this test exists for.
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("fa-IR");

            Assert.Equal("12.5", CardLayout.Css(12.5));
            Assert.Equal("0.75", CardLayout.Css(0.75));
            Assert.Equal("-3.25", CardLayout.Css(-3.25));
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public void Defaults_stay_inside_the_measured_safe_band()
    {
        // The whole point of the safe band is that text never lands on the painted flowers.
        const double top = 26, bottom = 62;
        var layout = CardLayoutDefaults.For((int)top, (int)bottom);

        Assert.NotEmpty(layout.Elements);
        foreach (var element in layout.Elements)
        {
            Assert.InRange(element.Y, top, bottom);
        }

        Assert.Equal(
            new[] { CardRole.Kicker, CardRole.Names, CardRole.Divider, CardRole.Message },
            layout.Elements.Select(e => e.Role));
    }

    [Fact]
    public void An_inverted_safe_band_is_repaired_rather_than_trusted()
    {
        // SafeBottom above SafeTop would otherwise produce a negative band and stack every
        // element on one line.
        var layout = CardLayoutDefaults.For(80, 10);

        foreach (var element in layout.Elements)
        {
            Assert.InRange(element.Y, 80d, 100d);
        }
    }

    [Fact]
    public void A_literal_colour_is_allowed_but_only_as_an_exact_hex()
    {
        // The editor lets people pick any colour. Safety here is a whitelist pattern, not a
        // search for dangerous characters - a blocklist would eventually miss one.
        const string json = """
            [{"id":"a","role":"text","style":{"color":"#C4463A"}},
             {"id":"b","role":"text","style":{"color":"#fff"}},
             {"id":"c","role":"text","style":{"color":"#ffffff;position:fixed"}},
             {"id":"d","role":"text","style":{"color":"accent"}}]
            """;

        Assert.True(CardLayout.TryParse(json, out var layout));
        var e = layout.Elements;

        Assert.Equal("#C4463A", e[0].Style.Color);
        Assert.True(e[0].Style.HasLiteralColor);

        // Three digits is a valid CSS colour but not the shape we accept, so it falls back.
        Assert.Equal("ink", e[1].Style.Color);
        Assert.Equal("ink", e[2].Style.Color);

        Assert.Equal("accent", e[3].Style.Color);
        Assert.False(e[3].Style.HasLiteralColor);
    }

    [Fact]
    public void Variants_are_validated_against_the_role_that_owns_them()
    {
        const string json = """
            [{"id":"a","role":"date","style":{"variant":"circle"}},
             {"id":"b","role":"date","style":{"variant":"arch"}},
             {"id":"c","role":"photo","style":{"variant":"oval"}},
             {"id":"d","role":"message","style":{"variant":"circle"}}]
            """;

        Assert.True(CardLayout.TryParse(json, out var layout));
        var e = layout.Elements;

        Assert.Equal("circle", e[0].Style.Variant);
        // "arch" belongs to photos, not dates, so the date falls back to its own default.
        Assert.Equal("plain", e[1].Style.Variant);
        Assert.Equal("oval", e[2].Style.Variant);
        // A role with no variants of its own can never carry one.
        Assert.Equal("plain", e[3].Style.Variant);
    }

    [Fact]
    public void A_painted_frame_can_only_name_a_slug()
    {
        const string json = """
            [{"id":"a","role":"photo","style":{"frame":"art","frameArt":"gol-e-sorkh"}},
             {"id":"b","role":"photo","style":{"frame":"art","frameArt":"../../appsettings"}},
             {"id":"c","role":"photo","style":{"frame":"art","frameArt":"x'); background:url(evil"}},
             {"id":"d","role":"photo","style":{"frame":"neon"}}]
            """;

        Assert.True(CardLayout.TryParse(json, out var layout));
        var e = layout.Elements;

        Assert.Equal("gol-e-sorkh", e[0].Style.FrameArt);
        // Both of these would otherwise end up inside a url() in the style attribute.
        Assert.Null(e[1].Style.FrameArt);
        Assert.Null(e[2].Style.FrameArt);
        Assert.Equal("none", e[3].Style.Frame);
    }

    [Fact]
    public void Feather_is_clamped_and_a_photo_always_has_a_height()
    {
        const string json = """
            [{"id":"a","role":"photo","style":{"feather":900}},
             {"id":"b","role":"photo","h":40}]
            """;

        Assert.True(CardLayout.TryParse(json, out var layout));

        Assert.Equal(100d, layout.Elements[0].Style.Feather);
        // A photo has no text to be sized by, so a missing height would collapse it to nothing.
        Assert.NotNull(layout.Elements[0].H);
        Assert.Equal(40d, layout.Elements[1].H);
    }

    [Fact]
    public void Focus_and_zoom_are_clamped_and_default_to_an_untouched_picture()
    {
        const string json = """
            [{"id":"a","role":"photo","style":{"focusX":-40,"focusY":999,"zoom":5000}},
             {"id":"b","role":"photo"},
             {"id":"c","role":"photo","style":{"focusX":18.5,"focusY":72,"zoom":140}}]
            """;

        Assert.True(CardLayout.TryParse(json, out var layout));
        var e = layout.Elements;

        // Anything outside the picture would move the crop off the image entirely.
        Assert.Equal(0d, e[0].Style.FocusX);
        Assert.Equal(100d, e[0].Style.FocusY);
        Assert.Equal(400d, e[0].Style.Zoom);

        // Untouched photos stay centred, exactly as every card saved before this existed.
        Assert.Equal(50d, e[1].Style.FocusX);
        Assert.Equal(50d, e[1].Style.FocusY);
        Assert.Equal(100d, e[1].Style.Zoom);

        Assert.Equal(18.5d, e[2].Style.FocusX);
        Assert.Equal(72d, e[2].Style.FocusY);
        Assert.Equal(140d, e[2].Style.Zoom);
    }
}
