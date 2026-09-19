namespace DigiCard.Web;

/// <summary>
/// Presentation-only labels for the DESIGN side of a template - the family it belongs to and
/// how it is composed. Occasion titles are not here: those are per-occasion data and are read
/// from the database, because the set of occasions changes without a deployment.
/// </summary>
public static class TemplateDisplay
{
    public static string FamilyTitle(string family) => family switch
    {
        "minimal" => "مینیمال",
        "watercolor" => "گل‌آرایی آبرنگی",
        "persian" => "کلاسیک ایرانی",
        "photo" => "عکس‌محور",
        _ => family,
    };

    public static string FamilyNote(string family) => family switch
    {
        "minimal" => "فضای سفید، خطوط نازک و تایپوگرافی آرام.",
        "watercolor" => "پس‌زمینه نرم و نقش‌مایه گل، با رنگ‌های ملایم.",
        "persian" => "قاب تزئینی، بته‌جقه و کاشی، با حال‌وهوای ایرانی.",
        "photo" => "جای عکس در بالای کارت و متن در پایین آن.",
        _ => "",
    };

    public static string LayoutTitle(string layout) => layout switch
    {
        "centered" => "چیدمان وسط‌چین",
        "banded" => "نوار بالای کارت",
        "arch" => "قاب گنبدی",
        "photo" => "جای عکس در بالا",
        _ => layout,
    };
}
