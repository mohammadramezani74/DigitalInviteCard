namespace DigiCard.Contracts.Templates;

/// <summary>
/// The arrangement a card starts with: the same stack of kicker, names, rule and message the
/// fixed markup used to draw, expressed as elements so the editor can take it apart.
///
/// Everything is placed inside the template's measured safe band, which is the strip of the
/// painted artwork with nothing in it. Placing by proportion rather than by fixed percentages
/// means a card whose flowers leave only a narrow band gets a tighter stack automatically,
/// instead of text landing on the painting.
/// </summary>
public static class CardLayoutDefaults
{
    /// <summary>Ids are fixed, not generated, so a template's seed data is byte-identical on every model build.</summary>
    public static CardLayout For(int safeTop, int safeBottom)
    {
        var top = CardLayout.Clamp(safeTop, 0, 90);
        var bottom = CardLayout.Clamp(safeBottom, top + 10, 100);
        var band = bottom - top;

        // Fractions of the band, measured off the cards we already have painted. The message is
        // given the tail of the band because it is the only part that wraps to several lines.
        // Rounded so the seeded JSON is short and identical on every platform.
        double At(double fraction) => Math.Round(top + band * fraction, 2);

        return CardLayout.FromElements(
        [
            new CardElement
            {
                Id = "kicker",
                Role = CardRole.Kicker,
                X = 10, Y = At(0.02), W = 80, Z = 10,
                Style = new CardElementStyle
                {
                    FontSize = 3.2, Font = "body", Weight = "normal",
                    Align = "center", Color = "muted", LineHeight = 1.6,
                },
            },
            new CardElement
            {
                Id = "names",
                Role = CardRole.Names,
                X = 6, Y = At(0.2), W = 88, Z = 20,
                Style = new CardElementStyle
                {
                    FontSize = 8.4, Font = "display", Weight = "bold",
                    Align = "center", Color = "ink", LineHeight = 1.35,
                },
            },
            new CardElement
            {
                Id = "divider",
                Role = CardRole.Divider,
                X = 32, Y = At(0.56), W = 36, H = 3, Z = 15,
                Style = new CardElementStyle { Align = "center", Color = "accent", Opacity = 0.8 },
            },
            new CardElement
            {
                Id = "message",
                Role = CardRole.Message,
                X = 12, Y = At(0.68), W = 76, Z = 10,
                Style = new CardElementStyle
                {
                    FontSize = 3.6, Font = "body", Weight = "normal",
                    Align = "center", Color = "ink", LineHeight = 1.9,
                },
            },
        ]);
    }

    /// <summary>The JSON a template row stores. Deterministic, so it is safe in EF seed data.</summary>
    public static string Json(int safeTop, int safeBottom) => For(safeTop, safeBottom).Serialize();
}
