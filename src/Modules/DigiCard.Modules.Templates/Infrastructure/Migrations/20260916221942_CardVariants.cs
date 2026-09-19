using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigiCard.Modules.Templates.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CardVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Poems",
                schema: "templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Poet = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    OccasionSlug = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poems", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "templates",
                table: "Poems",
                columns: new[] { "Id", "IsActive", "OccasionSlug", "Poet", "SortOrder", "Text" },
                values: new object[,]
                {
                    { new Guid("c3d52a10-0000-4000-9000-000000000001"), true, "", "حافظ", 1, "درخت دوستی بنشان که کام دل به بار آرد\nنهال دشمنی برکن که رنج بی‌شمار آرد" },
                    { new Guid("c3d52a10-0000-4000-9000-000000000002"), true, "", "حافظ", 2, "دست از طلب ندارم تا کام من برآید\nیا تن رسد به جانان یا جان ز تن برآید" },
                    { new Guid("c3d52a10-0000-4000-9000-000000000003"), true, "", "حافظ", 3, "مرا مهر سیه‌چشمان ز سر بیرون نخواهد شد\nقضای آسمان است این و دیگرگون نخواهد شد" },
                    { new Guid("c3d52a10-0000-4000-9000-000000000004"), true, "", "سعدی", 4, "به جهان خرم از آنم که جهان خرم از اوست\nعاشقم بر همه عالم که همه عالم از اوست" },
                    { new Guid("c3d52a10-0000-4000-9000-000000000005"), true, "", "سعدی", 5, "بنی‌آدم اعضای یک پیکرند\nکه در آفرینش ز یک گوهرند" },
                    { new Guid("c3d52a10-0000-4000-9000-000000000006"), true, "", "مولوی", 6, "عشق آن شعله است کاو چون برفروخت\nهر چه جز معشوق باقی جمله سوخت" },
                    { new Guid("c3d52a10-0000-4000-9000-000000000007"), true, "", "نظامی", 7, "به نام آنکه جان را فکرت آموخت\nچراغ دل به نور جان برافروخت" }
                });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0101"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":31.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":38.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":51.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8,\"variant\":\"rule\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":55.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0102"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":26.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":33.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":46.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8,\"variant\":\"rule\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":50.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0103"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":44.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":51.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":64.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8,\"variant\":\"rule\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":68.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0104"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":22.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":29.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":42.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8,\"variant\":\"rule\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":46.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0105"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":22.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":29.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":42.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8,\"variant\":\"rule\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":46.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0106"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":37.6,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":43,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":53.8,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8,\"variant\":\"rule\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":57.4,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0107"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":40.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":47.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":60.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8,\"variant\":\"rule\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":64.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0108"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":45.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":52.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":65.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8,\"variant\":\"rule\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":69.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0109"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":14.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":21.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":34.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8,\"variant\":\"rule\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":38.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010b"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":34.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":41.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":54.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8,\"variant\":\"rule\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":58.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010c"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":33.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":40.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":53.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8,\"variant\":\"rule\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":57.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1,\"variant\":\"plain\",\"digits\":\"fa\",\"frame\":\"none\",\"feather\":0,\"hasLiteralColor\":false}}]");

            migrationBuilder.CreateIndex(
                name: "IX_Poems_IsActive_OccasionSlug_SortOrder",
                schema: "templates",
                table: "Poems",
                columns: new[] { "IsActive", "OccasionSlug", "SortOrder" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Poems",
                schema: "templates");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0101"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":31.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":38.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":51.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":55.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0102"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":26.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":33.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":46.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":50.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0103"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":44.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":51.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":64.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":68.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0104"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":22.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":29.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":42.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":46.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0105"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":22.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":29.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":42.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":46.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0106"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":37.6,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":43,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":53.8,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":57.4,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0107"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":40.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":47.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":60.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":64.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0108"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":45.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":52.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":65.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":69.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0109"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":14.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":21.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":34.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":38.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010b"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":34.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":41.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":54.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":58.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1}}]");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010c"),
                column: "Elements",
                value: "[{\"id\":\"kicker\",\"role\":\"kicker\",\"x\":10,\"y\":33.72,\"w\":80,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.2,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"muted\",\"lineHeight\":1.6,\"opacity\":1}},{\"id\":\"names\",\"role\":\"names\",\"x\":6,\"y\":40.2,\"w\":88,\"rotation\":0,\"z\":20,\"style\":{\"fontSize\":8.4,\"font\":\"display\",\"weight\":\"bold\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.35,\"opacity\":1}},{\"id\":\"divider\",\"role\":\"divider\",\"x\":32,\"y\":53.16,\"w\":36,\"h\":3,\"rotation\":0,\"z\":15,\"style\":{\"fontSize\":4,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"accent\",\"lineHeight\":1.7,\"opacity\":0.8}},{\"id\":\"message\",\"role\":\"message\",\"x\":12,\"y\":57.48,\"w\":76,\"rotation\":0,\"z\":10,\"style\":{\"fontSize\":3.6,\"font\":\"body\",\"weight\":\"normal\",\"align\":\"center\",\"color\":\"ink\",\"lineHeight\":1.9,\"opacity\":1}}]");
        }
    }
}
