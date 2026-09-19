using System.Globalization;

namespace DigiCard.Contracts.Templates;

/// <summary>
/// Jalali dates for cards.
///
/// The card stores a real date, not a formatted string. That is the whole reason this exists: the
/// same wedding date has to be able to appear as "جمعه، ۲۴ مرداد ۱۴۰۵" on one template and as
/// "۱۴۰۵/۰۵/۲۴" inside a ring on another, and a stored string can only ever be one of those.
///
/// Storage is the Gregorian date, because that is what SQL Server, sorting and any future
/// reminder job understand. The Jalali form is produced here at the edge.
/// </summary>
public static class PersianDate
{
    private static readonly PersianCalendar Calendar = new();

    public static readonly string[] MonthNames =
    [
        "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
        "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند",
    ];

    // Indexed by System.DayOfWeek, which starts at Sunday.
    private static readonly string[] WeekdayNames =
    [
        "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه", "شنبه",
    ];

    /// <summary>A date split into the pieces a card design needs separately.</summary>
    public sealed record Parts(int Year, int Month, int Day, string MonthName, string Weekday);

    public static Parts Split(DateOnly date)
    {
        var moment = date.ToDateTime(TimeOnly.MinValue);
        var month = Calendar.GetMonth(moment);

        return new Parts(
            Calendar.GetYear(moment),
            month,
            Calendar.GetDayOfMonth(moment),
            MonthNames[month - 1],
            WeekdayNames[(int)moment.DayOfWeek]);
    }

    /// <summary>
    /// Converts a Jalali date the user typed. Returns false for a date that does not exist - the
    /// obvious one being 30 Esfand in a common year, which a naive converter turns into 1 Farvardin
    /// of the next year and silently moves someone's wedding.
    /// </summary>
    public static bool TryFromJalali(int year, int month, int day, out DateOnly date)
    {
        date = default;
        if (year < 1 || month < 1 || month > 12 || day < 1 || day > 31) return false;

        try
        {
            date = DateOnly.FromDateTime(Calendar.ToDateTime(year, month, day, 0, 0, 0, 0));
            return true;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }

    /// <summary>
    /// Whether a Jalali year has a 30th of Esfand. The editor's date picker needs this so it can
    /// stop offering a day that does not exist rather than letting the conversion reject it.
    /// </summary>
    public static bool IsLeapYear(int year) => year >= 1 && Calendar.IsLeapYear(year);

    /// <summary>Renders a date for the variants that are a single line of text.</summary>
    public static string Format(DateOnly date, string variant, bool persianDigits)
    {
        var p = Split(date);

        var text = variant switch
        {
            "weekday" => $"{p.Weekday}، {p.Day} {p.MonthName} {p.Year}",
            "numeric" => $"{p.Year}/{p.Month:00}/{p.Day:00}",
            _ => $"{p.Day} {p.MonthName} {p.Year}",
        };

        return persianDigits ? ToPersianDigits(text) : text;
    }

    public static string FormatTime(TimeOnly time, bool persianDigits)
    {
        var text = $"{time.Hour:00}:{time.Minute:00}";
        return persianDigits ? ToPersianDigits(text) : text;
    }

    /// <summary>
    /// Swaps ASCII digits for Persian ones. Done on the finished string rather than with a culture
    /// so that the separators stay exactly as written - a culture-formatted date would also change
    /// the slash and the ordering, which is not what is wanted here.
    /// </summary>
    public static string ToPersianDigits(string text)
    {
        Span<char> buffer = stackalloc char[text.Length];
        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];
            buffer[i] = c is >= '0' and <= '9' ? (char)('۰' + (c - '0')) : c;
        }

        return new string(buffer);
    }
}
