# دیجی‌کارت — اسکلت توسعه

پایه توسعه با .NET 10، Blazor Web App، SQL Server و معماری Modular Monolith + Vertical Slice.
این بسته قالب شروع توسعه است، نه محصول آماده فروش. حساب‌ها از قالب رسمی Identity مایکروسافت اقتباس شده‌اند؛ صفحات حساب فعلاً انگلیسی‌اند.

## اجرای سریع روی ویندوز

۱. .NET 10 SDK و SQL Server Express LocalDB را نصب کنید. برای کار در Visual Studio از نسخه‌ای با پشتیبانی .NET 10 استفاده کنید؛ اجرای CLI هم ممکن است.
۲. فایل DigiCard.sln را باز کنید. پروژه شروع DigiCard.Web است.
۳. در پوشه اصلی پروژه اجرا کنید:

```powershell
dotnet restore DigiCard.sln
dotnet tool restore
dotnet run --project src/DigiCard.DbMigrator
dotnet dev-certs https --trust
dotnet run --project src/DigiCard.Web --launch-profile https
```

آدرس اجرا در خروجی کنسول نمایش داده می‌شود. در `/Account/Register` حساب بسازید. در محیط Development لینک تأیید حساب روی صفحه تأیید ثبت‌نام نمایش داده می‌شود؛ آن را باز کنید و سپس وارد شوید. در `/templates` قالب انتخاب کنید، متن را تغییر دهید و پیش‌نویس بسازید. صفحه `/drafts/{id}` فقط برای صاحب پیش‌نویس قابل مشاهده است.

اطلاعات دو قالب نمونه با migration درج می‌شود. این دو قالب نمایشی فقط رنگ متفاوت دارند؛ طراحی هنری قالب‌های نهایی جزو این بسته نیست.

## SQL Server نصب‌شده یا Docker

برای اتصال به سرور خودتان، این متغیر را در همان ترمینال اجرای migrator و وب تنظیم کنید:

```powershell
$env:ConnectionStrings__DefaultConnection = 'Server=localhost;Database=DigiCard;Trusted_Connection=True;TrustServerCertificate=True'
dotnet run --project src/DigiCard.DbMigrator
dotnet run --project src/DigiCard.Web --launch-profile https
```

برای Docker، `.env.example` را به `.env` کپی کنید و رمز قوی دلخواه بگذارید:

```powershell
docker compose up -d
$env:ConnectionStrings__DefaultConnection = 'Server=localhost,1433;Database=DigiCard;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True'
dotnet run --project src/DigiCard.DbMigrator
dotnet run --project src/DigiCard.Web --launch-profile https
```

قبل از اجرای migrator منتظر آماده‌شدن SQL Server بمانید. تصویر Docker از SQL Server 2022 Developer استفاده می‌کند و برای توسعه است. برای محیط عملیاتی از کاربر برنامه با دسترسی محدود، گواهی معتبر و روش مدیریت رمز مناسب استفاده کنید. رمزها را در git ثبت نکنید. `TrustServerCertificate=True` در نمونه‌ها مخصوص توسعه محلی است.

## پروژه‌ها

| پروژه | مسئولیت |
|---|---|
| DigiCard.Web | میزبان ASP.NET Core، صفحات SSR، Identity UI و ترکیب ماژول‌ها |
| DigiCard.Web.Client | ویرایشگر WebAssembly و کامپوننت پیش‌نمایش |
| DigiCard.Contracts | قراردادهای کوچک مشترک؛ بدون EF یا منطق سرور |
| DigiCard.Modules.Accounts | Identity و schema حساب‌ها |
| DigiCard.Modules.Templates | کاتالوگ قالب‌ها و نسخه قالب |
| DigiCard.Modules.Invitations | قوانین پیش‌نویس و اسلایس‌های ساخت و خواندن |
| DigiCard.DbMigrator | اجرای صریح migrationها و کارخانه‌های design-time |
| DigiCard.Tests | تست‌های دامنه، API و تست یکپارچه SQL Server |

صفحات `.razor` در این پروژه کامپوننت Blazor هستند. پروژه Razor Pages یا صفحه `.cshtml` نداریم. صفحات عمومی SSR هستند و ویرایشگر، WebAssembly با prerender غیرفعال است. بنابراین ویرایشگر تا دریافت runtime مرورگر وضعیت اولیه ساده دارد.

## تست

```powershell
dotnet test DigiCard.sln -c Release
```

تست SQL Server بدون متغیر زیر صریحاً Skip می‌شود. آن را فقط به دیتابیس اختصاصی تست وصل کنید؛ تست migration اجرا می‌کند و داده نمونه می‌سازد.

```powershell
$env:DIGICARD_TEST_SQL = 'Server=(localdb)\mssqllocaldb;Database=DigiCardTests;Trusted_Connection=True;TrustServerCertificate=True'
dotnet test DigiCard.sln -c Release
```

CI نمونه برای GitHub همراه SQL Server ارائه شده است؛ تا قبل از اجرای آن در ریپازیتوری شما، سبزبودن CI ادعا نمی‌شود. وضعیت بررسی همین بسته را در `docs/VALIDATION.md` ببینید.

## افزودن migration

فرمان نمونه برای ماژول دعوت‌نامه‌ها:

```powershell
dotnet ef migrations add AddInvitationContent --project src/Modules/DigiCard.Modules.Invitations --startup-project src/DigiCard.DbMigrator --context InvitationsDbContext --output-dir Infrastructure/Migrations
dotnet run --project src/DigiCard.DbMigrator
```

نام تغییر را مطابق قابلیت واقعی انتخاب کنید. هر ماژول DbContext، schema و تاریخچه migration خودش را دارد. وب به‌صورت خودکار دیتابیس را تغییر نمی‌دهد. migrationها به ترتیب accounts، templates و invitations اجرا می‌شوند؛ این سه اجرا یک تراکنش مشترک ندارند.

## محدوده آماده

- گالری SSR با دو نمونه؛ انتخاب قالب به ویرایشگر منتقل می‌شود.
- تغییر زنده نام‌ها و متن در مرورگر؛ محدودیت طول متن سمت سرور.
- ساخت و ذخیره پیش‌نویس و بازیابی خصوصی با کنترل مالکیت.
- Cookie authentication، کنترل CSRF در عملیات نوشتن و no-store برای پاسخ خصوصی.
- ذخیره شناسه و نسخه قالب، رنگ قالب و rowversion برای توسعه کنترل هم‌زمانی.
- EF Core SQL Server، migrationهای اولیه، نسخه‌های متمرکز پکیج‌ها و تنظیمات توسعه.

## مراحل بعدی

ویرایش پیش‌نویس ذخیره‌شده و کنترل تعارض، قالب‌های لایه‌ای واقعی، آپلود/برش عکس، تاریخ مراسم، پاکت و موسیقی، انتشار نسخه مستقل، مهمانان و RSVP، پرداخت و ارسال پیام هنوز پیاده نشده‌اند. دکمه فعلی پیش‌نویس جدید می‌سازد؛ API به‌صورت idempotent طراحی نشده و در قطع ارتباط مبهم، ارسال مجدد می‌تواند رکورد دیگری بسازد.

در Production اجرای وب عمداً تا جایگزینی ایمیل‌فرست آزمایشی متوقف می‌شود. در Program.cs توضیح محل اتصال IEmailSender واقعی آمده است؛ پس از اتصال و آزمون آن، guard را حذف کنید. بومی‌سازی و OTP، محدودسازی نرخ درخواست، نگهداری کلیدهای Data Protection، تنظیم AllowedHosts و نظارت عملیاتی نیز قبل از انتشار باید تکمیل شوند.

برای ادامه کار، `docs/ARCHITECTURE.md` و `docs/NEXT-STEPS.md` را بخوانید.

## استفاده مجدد به‌عنوان قالب dotnet new

برای ادامه همین پروژه کافی است solution را باز کنید. اگر پروژه تازه‌ای از همین ساختار می‌خواهید، از پوشه اصلی این بسته اجرا کنید:

```powershell
dotnet new install .
dotnet new digicard -n MyWeddingApp -o ../MyWeddingApp
```

نام پروژه و namespaceهای دارای DigiCard تغییر می‌کنند. فایل‌های خروجی build و رمزهای محلی در قالب کپی نمی‌شوند. مقدارهای ثابت شناسه دو قالب نمونه، وابسته به پروژه جدید نیستند و به‌صورت seed همان دیتابیس استفاده می‌شوند.
