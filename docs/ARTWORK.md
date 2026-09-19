# تولید تصویر قالب‌ها — مشخصات و پرامپت

هدف: هر قالب یک تصویر تزئینی زیبا داشته باشد که **هیچ متنی در آن نیست**، و متن کارت را کد روی آن رندر کند.

---

## قواعد ثابت برای همه تصویرها

| مورد | مقدار |
|---|---|
| اندازه | ۱۰۲۴ × ۱۵۳۶ پیکسل (نسبت ۲:۳، عمودی) |
| فرمت خروجی از GPT | PNG |
| متن داخل تصویر | **ممنوع** — هیچ حرف، عدد، امضا یا واترمارک |
| آدم و چهره | ممنوع |
| ناحیه امن وسط | از ۱۸٪ تا ۸۲٪ عرض، از ۲۸٪ تا ۸۰٪ ارتفاع: آرام، روشن، کم‌جزئیات |
| محل تزئینات | گوشه‌ها و لبه‌ها |
| لبه‌ها | تا لبه کادر ادامه پیدا کند، بدون حاشیه سفید و بدون سایه کارت |

اگر تصویر وسطش شلوغ باشد، متن رویش خوانا نمی‌شود و آن قالب دور ریخته می‌شود. این مهم‌ترین بند است.

---

## اول فقط یک تصویر بسازید

قبل از ساخت دوازده‌تا، **فقط قالب اول را بسازید** و بدهید تا در پروژه بگذارم. اگر خط لوله درست کار کرد و روی کارت خوب نشست، بقیه را بسازید. اگر اول دوازده‌تا بسازید و بعد معلوم شود ناحیه امن کم است یا رنگ‌ها نمی‌خوانند، هر دوازده‌تا باید دوباره ساخته شوند.

---

## پرامپت قالب ۱ — «گلاب آبرنگ»

```
A vertical 2:3 watercolor illustration for a LUXURY wedding invitation background.
1024 x 1536 pixels.

Quality: fine-art quality, editorial, exquisite and romantic. The kind of artwork
sold by a high-end wedding stationery house. Painted by a master watercolorist with
a confident, loose, expressive brush - beautiful, refined and emotionally warm.
Museum-grade paper, luminous pigment, exceptional craftsmanship in every petal.

Style: delicate hand-painted watercolor on soft cotton rag paper, visible paper grain,
soft pigment bleeds and granulation, gentle wet-on-wet edges, subtle blooms where the
water pools, a few fine ink details. Airy, elegant and light-filled.
NOT saturated, NOT digital-looking, NOT vector, NOT cartoon, NOT clipart, NOT 3D.

Composition: a loose floral spray of garden roses, small blossoms and fine eucalyptus
leaves entering from the TOP-LEFT corner and a smaller answering spray at the
BOTTOM-RIGHT corner. The two corners are diagonally balanced.
The ENTIRE CENTER of the image must stay almost empty: a calm, very pale wash with
no detail, so that text can be placed over it and stay readable.

Palette: dusty rose, blush pink, soft sage green, warm cream background.
Muted and desaturated throughout.

The artwork must bleed to all four edges. No border, no frame, no card mockup,
no drop shadow, no background behind the artwork.

Absolutely no text, no letters, no words, no numbers, no signature, no watermark,
no logo. No people, no faces, no hands.

Make it genuinely beautiful - this is the single image the whole product is judged on.
```

اگر خروجی اول را پسندیدید ولی کامل نبود، به GPT بگویید
`make it more delicate and more luxurious, keep the center empty`
و دوباره بسازد. معمولاً نسخه دوم یا سوم خیلی بهتر از اولی است؛ چند بار تکرار کنید و بهترین را بردارید.

---

## پرامپت‌های بقیه قالب‌ها

همان قالب بالا را بردارید و فقط دو خط **Composition** و **Palette** را عوض کنید. بقیه بندها باید کلمه‌به‌کلمه یکسان بمانند — این چیزی است که دوازده تصویر را به یک خانواده بصری منسجم تبدیل می‌کند به‌جای دوازده تصویر بی‌ربط.

| قالب | Composition | Palette |
|---|---|---|
| شقایق | poppies and wild grasses along the BOTTOM edge only, center empty | soft coral red, warm sand, pale olive |
| بنفشه | violets and small leaves in a narrow vertical band down the LEFT edge | dusty violet, lilac, pale grey-green, ivory |
| حنا و گل | marigold and jasmine garland across the TOP edge, thin trailing stems down both sides | warm saffron, marigold orange, soft ivory |
| سپید ساده | a single very pale botanical branch at the TOP-RIGHT, extremely minimal, mostly empty warm white paper | warm off-white, pale taupe, faint sage |
| خط نور | a soft gradient wash from the top edge fading to white, no flowers at all | warm cream, pale gold, soft beige |
| سادگی طلایی | thin delicate gold-leaf botanical sprigs at TOP and BOTTOM, very sparse | ivory, antique gold, pale champagne |
| باغ ایرانی | a Persian garden motif with stylised cypress and blossom branches framing LEFT and RIGHT edges | deep sage green, terracotta, cream |
| ترمه | a Persian termeh paisley border along TOP and BOTTOM edges, painted in watercolor | deep madder red, warm ochre, cream |
| کاشی فیروزه | a Persian tile pattern border at the TOP edge and a narrow matching band at the BOTTOM | turquoise, deep teal, ivory |
| رز و مروارید | roses and pearls in the LOWER THIRD only, the upper half left as plain soft wash for a photo | blush pink, pearl grey, warm white |
| قاب خاطره | a soft painted rectangular mat with irregular watercolor edges, the inner area left plain | cool grey, soft slate, warm white |

قالب‌های «رز و مروارید» و «قاب خاطره» عکس‌محورند؛ بالای تصویرشان باید خالی بماند چون جای عکس کاربر است.

---

## بعد از ساخت

فایل‌های PNG را با همین نام‌ها در این پوشه بگذارید:

```
DigiCard/artwork-input/
    golab-abrang.png
    shaghayegh.png
    banafsheh.png
    hana-o-gol.png
    sepid-sade.png
    khat-e-noor.png
    sadegi-talaei.png
    bagh-e-irani.png
    termeh.png
    kashi-firouzeh.png
    roz-o-morvarid.png
    ghab-e-khatereh.png
```

نام فایل باید دقیقاً برابر `Slug` همان قالب باشد. بقیه‌اش با من است: تبدیل به WebP، ساخت نسخه کوچک برای گالری، اضافه‌کردن لایه دارایی به مدل قالب و migration.

---

## چه چیزی بعد از این از سمت کد قابل تغییر می‌ماند

تصویر ثابت است، ولی این‌ها همچنان در اختیار کد و ویرایشگر می‌مانند:

- نام عروس و داماد، متن دعوت و متن بالای کارت
- فونت، اندازه، وزن و رنگ هر کدام از این‌ها
- محل بلوک متن و ناحیه امن، جدا برای هر قالب
- رنگ تأکید کارت
- در قالب‌های عکس‌محور، عکس کاربر که بین تصویر پس‌زمینه و متن می‌نشیند

چیزی که قابل تغییر نیست خود نقاشی است. برای همین است که متن نباید داخلش پخته شود.
