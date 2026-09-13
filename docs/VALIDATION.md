# بررسی بسته

## بررسی‌های موفق

- ساخت Release پروژه‌های Contracts، Accounts، Templates، Invitations و DbMigrator با .NET SDK 10.0.401: بدون هشدار یا خطا.
- کامپایل C# و Razor پروژه Web.Client و Web با هدف MSBuild Compile: موفق. فرم‌های Identity برای مقداردهی امن SupplyParameterFromForm در .NET 10 اصلاح شدند.
- ساخت اسمبلی تست: بدون هشدار یا خطا.
- اجرای xUnit: ۱۰ تست موفق، صفر شکست، یک تست SQL Server اجرا نشده (Skip).
- تست‌های اجراشده: محدودیت نام و مالک، حفظ نسخه قالب، نمایش SSR صفحه اصلی/ورود/ثبت‌نام، رد خواندن ناشناس، رد CSRF نامعتبر و رد ورودی ناقص.
- تولید migrationهای اولیه هر سه ماژول و اسکریپت‌های idempotent SQL Server با EF Core 10.0.12: موفق. اسکریپت‌ها در docs/sql هستند؛ در SSMS ابتدا دیتابیس هدف را انتخاب و به ترتیب اجرا کنید. اجرای DbMigrator راه پیشنهادی است.
- نصب قالب dotnet new و تولید پروژه با نام متفاوت: موفق.

## محدودیت بررسی

ساخت کامل solution در این محیط هنگام task مربوط به بسته‌بندی WebAssembly متوقف شد: MSB4216 / ComputeWasmBuildAssets به دلیل عدم ایجاد یا اتصال به MSBuild task host. برای بررسی خطاهای کد، هدف Compile و انتقال خروجی اسمبلی‌ها جدا اجرا شدند و تست‌ها روی این اسمبلی‌ها اجرا شدند. این نتیجه جای تأیید full build یا publish را نمی‌گیرد.

SQL Server و Docker اجرایی در این محیط موجود نبودند؛ migration روی سرور واقعی، ذخیره و بازیابی واقعی دیتابیس، تست یکپارچه مالکیت با SQL Server و اجرای مرورگری WebAssembly تأیید نشده‌اند. تست SQL Server در بسته و CI وجود دارد و با DIGICARD_TEST_SQL فعال می‌شود.

بعد از استخراج روی ویندوز، full build و تست SQL Server طبق README.fa.md را اجرا کنید. Workflow GitHub فقط آماده شده و هنوز روی ریپازیتوری کاربر اجرا نشده است. هیچ نتیجه عملیاتی یا آمادگی برای فروش ادعا نمی‌شود.
