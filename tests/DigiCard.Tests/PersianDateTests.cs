using DigiCard.Contracts.Templates;

namespace DigiCard.Tests;

/// <summary>
/// Calendar conversion is the kind of code that looks right and is wrong by one day, and a card
/// that shows the wrong day is worse than a card that fails to render. These tests are mostly
/// round-trips rather than hand-written pairs, so they check the property that actually matters
/// instead of one date someone remembered.
/// </summary>
public sealed class PersianDateTests
{
    [Fact]
    public void Nowruz_1400_is_21_March_2021()
    {
        var parts = PersianDate.Split(new DateOnly(2021, 3, 21));

        Assert.Equal(1400, parts.Year);
        Assert.Equal(1, parts.Month);
        Assert.Equal(1, parts.Day);
        Assert.Equal("فروردین", parts.MonthName);
    }

    [Fact]
    public void Every_day_across_five_years_round_trips()
    {
        var date = new DateOnly(2021, 3, 21);

        for (var i = 0; i < 365 * 5; i++, date = date.AddDays(1))
        {
            var parts = PersianDate.Split(date);

            Assert.True(PersianDate.TryFromJalali(parts.Year, parts.Month, parts.Day, out var back),
                $"{parts.Year}/{parts.Month}/{parts.Day} was produced by Split but rejected on the way back.");
            Assert.Equal(date, back);
        }
    }

    [Fact]
    public void A_day_that_does_not_exist_is_refused_not_rolled_over()
    {
        // The 30th of Esfand exists only in a leap year. Left to itself, a conversion happily
        // turns it into the 1st of Farvardin of the next year - which moves a wedding by a day
        // and gives the guests the wrong date.
        var commonYear = Enumerable.Range(1400, 12).First(y => !PersianDate.IsLeapYear(y));

        Assert.False(PersianDate.TryFromJalali(commonYear, 12, 30, out _));
    }

    [Fact]
    public void The_thirtieth_of_esfand_works_in_a_leap_year()
    {
        var leapYear = Enumerable.Range(1400, 12).First(PersianDate.IsLeapYear);

        Assert.True(PersianDate.TryFromJalali(leapYear, 12, 30, out var date));
        Assert.Equal((leapYear, 12, 30), (PersianDate.Split(date).Year, PersianDate.Split(date).Month, PersianDate.Split(date).Day));
    }

    [Theory]
    [InlineData(1404, 0, 10)]
    [InlineData(1404, 13, 10)]
    [InlineData(1404, 5, 0)]
    [InlineData(1404, 5, 32)]
    [InlineData(0, 5, 10)]
    public void Out_of_range_parts_are_refused(int year, int month, int day) =>
        Assert.False(PersianDate.TryFromJalali(year, month, day, out _));

    [Fact]
    public void Variants_read_the_way_the_designs_need_them()
    {
        PersianDate.TryFromJalali(1404, 5, 24, out var date);

        Assert.Equal("۲۴ مرداد ۱۴۰۴", PersianDate.Format(date, "plain", persianDigits: true));
        Assert.Equal("1404/05/24", PersianDate.Format(date, "numeric", persianDigits: false));
        Assert.StartsWith(PersianDate.Split(date).Weekday, PersianDate.Format(date, "weekday", persianDigits: true));

        // An unknown variant must not blow up - it reads as the plain one.
        Assert.Equal(PersianDate.Format(date, "plain", true), PersianDate.Format(date, "nonsense", true));
    }

    [Fact]
    public void Only_digits_are_swapped_never_the_separators()
    {
        Assert.Equal("۱۴۰۴/۰۵/۲۴", PersianDate.ToPersianDigits("1404/05/24"));
        Assert.Equal("۱۸:۳۰", PersianDate.ToPersianDigits("18:30"));
        Assert.Equal("مرداد ۱۴۰۴", PersianDate.ToPersianDigits("مرداد 1404"));
    }

    [Fact]
    public void Time_is_rendered_as_a_wall_clock()
    {
        Assert.Equal("۰۷:۰۵", PersianDate.FormatTime(new TimeOnly(7, 5), persianDigits: true));
        Assert.Equal("19:30", PersianDate.FormatTime(new TimeOnly(19, 30), persianDigits: false));
    }
}
