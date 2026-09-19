using DigiCard.Contracts.Templates;

namespace DigiCard.Web.Client.Editor;

/// <summary>
/// Everything the editor knows: the layout being edited, what is selected, and how to get back.
///
/// Undo is almost free here because a layout is an immutable record - a history entry is the whole
/// layout, not a diff, so there is no replay logic to get wrong. The only care needed is when to
/// push: once per gesture, not once per pointer move, or dragging a name across the card would
/// bury fifty entries in the stack.
///
/// Every mutation goes back through CardLayout.FromElements, so the editor cannot put a value on
/// screen that the server would later reject - what you see is already normalized.
/// </summary>
public sealed class EditorState
{
    private const int HistoryLimit = 50;

    private readonly List<CardLayout> undo = [];
    private readonly List<CardLayout> redo = [];

    private bool gestureOpen;

    public CardLayout Layout { get; private set; } = CardLayout.Empty;

    public string? SelectedId { get; private set; }

    public bool IsDirty { get; private set; }

    /// <summary>Raised whenever the layout or the selection changed and the UI should redraw.</summary>
    public event Action? Changed;

    public CardElement? Selected =>
        SelectedId is null ? null : Layout.Elements.FirstOrDefault(e => e.Id == SelectedId);

    public bool CanUndo => undo.Count > 0;
    public bool CanRedo => redo.Count > 0;

    public string Json => Layout.Serialize();

    /// <summary>Starts editing a layout. Clears the history: this is a different card.</summary>
    public void Load(string? json)
    {
        CardLayout.TryParse(json, out var layout);
        Layout = layout;
        SelectedId = null;
        undo.Clear();
        redo.Clear();
        IsDirty = false;
        Changed?.Invoke();
    }

    public void Select(string? id)
    {
        if (SelectedId == id) return;
        SelectedId = id;
        Changed?.Invoke();
    }

    // ------------------------------------------------------------------ history
    /// <summary>
    /// Opens a gesture: one undo entry covers everything until <see cref="EndGesture"/>. A drag
    /// calls this on pointer down, so the whole drag undoes in one step.
    /// </summary>
    public void BeginGesture()
    {
        if (gestureOpen) return;
        PushHistory();
        gestureOpen = true;
    }

    public void EndGesture() => gestureOpen = false;

    private void PushHistory()
    {
        undo.Add(Layout);
        if (undo.Count > HistoryLimit) undo.RemoveAt(0);
        redo.Clear();
    }

    public void Undo()
    {
        if (undo.Count == 0) return;

        redo.Add(Layout);
        Layout = undo[^1];
        undo.RemoveAt(undo.Count - 1);
        KeepSelectionValid();
        IsDirty = true;
        Changed?.Invoke();
    }

    public void Redo()
    {
        if (redo.Count == 0) return;

        undo.Add(Layout);
        Layout = redo[^1];
        redo.RemoveAt(redo.Count - 1);
        KeepSelectionValid();
        IsDirty = true;
        Changed?.Invoke();
    }

    private void KeepSelectionValid()
    {
        if (SelectedId is not null && Layout.Elements.All(e => e.Id != SelectedId))
            SelectedId = null;
    }

    // ------------------------------------------------------------------ mutation
    /// <summary>
    /// Changes the selected element. Outside a gesture this is one undo step on its own, which is
    /// what a click on a colour swatch should be.
    /// </summary>
    public void Update(Func<CardElement, CardElement> change)
    {
        if (Selected is null) return;
        if (!gestureOpen) PushHistory();

        var id = SelectedId;
        Layout = CardLayout.FromElements(
            Layout.Elements.Select(e => e.Id == id ? change(e) : e));

        IsDirty = true;
        Changed?.Invoke();
    }

    public void UpdateStyle(Func<CardElementStyle, CardElementStyle> change) =>
        Update(e => e with { Style = change(e.Style) });

    /// <summary>
    /// Adds an element of a role. Bound roles that are already on the card are refused: two
    /// "names" boxes would both show the same names and neither would be the real one.
    /// </summary>
    public bool Add(string role)
    {
        if (!CanAdd(role)) return false;
        PushHistory();

        var element = NewElement(role, TopZ() + 1);
        Layout = CardLayout.FromElements(Layout.Elements.Append(element));
        SelectedId = Layout.Elements[^1].Id;

        IsDirty = true;
        Changed?.Invoke();
        return true;
    }

    public bool CanAdd(string role)
    {
        if (Layout.Elements.Count >= CardLayout.MaxElements) return false;

        // Free roles can repeat - two verses on one card is a real thing. Bound ones cannot.
        return CardRole.CarriesOwnText(role) || Layout.Elements.All(e => e.Role != role);
    }

    public void Remove()
    {
        if (Selected is null) return;
        PushHistory();

        var id = SelectedId;
        Layout = CardLayout.FromElements(Layout.Elements.Where(e => e.Id != id));
        SelectedId = null;

        IsDirty = true;
        Changed?.Invoke();
    }

    /// <summary>
    /// Turns the combined names block into two name boxes with the joining word between them, so
    /// each can be moved, sized and coloured on its own. Laid out where the block already was:
    /// the first name to the right, because the card reads right to left.
    /// </summary>
    public bool SplitNames()
    {
        var names = Layout.Elements.FirstOrDefault(e => e.Role == CardRole.Names);
        if (names is null) return false;

        PushHistory();

        var parts = new[]
        {
            names with
            {
                Id = "name1", Role = CardRole.Name1,
                X = names.X + names.W * 0.54, W = names.W * 0.46,
            },
            names with
            {
                Id = "joiner", Role = CardRole.Joiner,
                X = names.X + names.W * 0.44, W = names.W * 0.12,
                // The joining word is a quiet connector, not a third name.
                Style = names.Style with
                {
                    FontSize = names.Style.FontSize * 0.42,
                    Font = "body",
                    Weight = "normal",
                    Color = "accent",
                },
            },
            names with
            {
                Id = "name2", Role = CardRole.Name2,
                X = names.X, W = names.W * 0.46,
            },
        };

        Layout = CardLayout.FromElements(
            Layout.Elements.Where(e => e.Role != CardRole.Names).Concat(parts));

        SelectedId = "name1";
        IsDirty = true;
        Changed?.Invoke();
        return true;
    }

    /// <summary>Puts the two names back into one block, spanning whatever they now cover.</summary>
    public bool MergeNames()
    {
        var parts = Layout.Elements.Where(e => CardRole.SplitNameRoles.Contains(e.Role)).ToList();
        if (parts.Count == 0) return false;

        PushHistory();

        var first = parts.FirstOrDefault(e => e.Role == CardRole.Name1) ?? parts[0];
        var left = parts.Min(e => e.X);
        var right = parts.Max(e => e.X + e.W);

        var merged = first with
        {
            Id = "names",
            Role = CardRole.Names,
            X = left,
            W = right - left,
            Y = parts.Min(e => e.Y),
        };

        Layout = CardLayout.FromElements(
            Layout.Elements.Where(e => !CardRole.SplitNameRoles.Contains(e.Role)).Append(merged));

        SelectedId = "names";
        IsDirty = true;
        Changed?.Invoke();
        return true;
    }

    public bool HasCombinedNames => Layout.Elements.Any(e => e.Role == CardRole.Names);

    public bool HasSplitNames => Layout.Elements.Any(e => CardRole.SplitNameRoles.Contains(e.Role));

    public void Raise() => Restack(+1);
    public void Lower() => Restack(-1);

    private void Restack(int direction)
    {
        if (Selected is null) return;
        Update(e => e with { Z = Math.Clamp(e.Z + direction * 10, 0, 999) });
    }

    private int TopZ() => Layout.Elements.Count == 0 ? 10 : Layout.Elements.Max(e => e.Z);

    /// <summary>
    /// A new element lands in the middle of the card rather than at a corner, because the first
    /// thing anyone does with a new box is drag it - and a box under the edge is hard to grab.
    /// </summary>
    private CardElement NewElement(string role, int z)
    {
        var id = UniqueId(role);

        return role switch
        {
            CardRole.Poem => new CardElement
            {
                Id = id, Role = role, X = 12, Y = 55, W = 76, Z = z,
                Text = "دو دل یک شد و یک دل، دو جهان روشن شد",
                Style = new CardElementStyle
                {
                    FontSize = 3.6, Font = "nastaliq", Align = "center",
                    Color = "accent", LineHeight = 2.1,
                },
            },
            CardRole.Photo => new CardElement
            {
                Id = id, Role = role, X = 32, Y = 12, W = 36, H = 24, Z = z,
                Style = new CardElementStyle { Variant = "circle", Frame = "thin" },
            },
            CardRole.Date => new CardElement
            {
                Id = id, Role = role, X = 20, Y = 72, W = 60, Z = z,
                Style = new CardElementStyle { FontSize = 3.4, Color = "accent", Variant = "plain" },
            },
            CardRole.Time => new CardElement
            {
                Id = id, Role = role, X = 35, Y = 79, W = 30, Z = z,
                Style = new CardElementStyle { FontSize = 3.2, Color = "accent" },
            },
            CardRole.Address => new CardElement
            {
                Id = id, Role = role, X = 12, Y = 84, W = 76, Z = z,
                Style = new CardElementStyle { FontSize = 2.9, Opacity = 0.85 },
            },
            CardRole.Divider => new CardElement
            {
                Id = id, Role = role, X = 32, Y = 50, W = 36, H = 3, Z = z,
                Style = new CardElementStyle { Color = "accent", Variant = "rule" },
            },
            _ => new CardElement
            {
                Id = id, Role = CardRole.Text, X = 15, Y = 45, W = 70, Z = z,
                Text = "متن شما",
                Style = new CardElementStyle { FontSize = 3.6 },
            },
        };
    }

    private string UniqueId(string role)
    {
        if (Layout.Elements.All(e => e.Id != role)) return role;

        for (var n = 2; ; n++)
        {
            var candidate = $"{role}-{n}";
            if (Layout.Elements.All(e => e.Id != candidate)) return candidate;
        }
    }

    public void MarkSaved()
    {
        IsDirty = false;
        Changed?.Invoke();
    }
}
