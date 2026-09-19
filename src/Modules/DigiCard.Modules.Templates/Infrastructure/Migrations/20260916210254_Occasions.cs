using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigiCard.Modules.Templates.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Occasions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Occasions",
                schema: "templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Tagline = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PrimaryLabel = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    SecondaryLabel = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Joiner = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DefaultKicker = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    DefaultMessage = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occasions", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "templates",
                table: "Occasions",
                columns: new[] { "Id", "DefaultKicker", "DefaultMessage", "Icon", "IsActive", "Joiner", "PrimaryLabel", "SecondaryLabel", "Slug", "SortOrder", "Tagline", "Title" },
                values: new object[,]
                {
                    { new Guid("b7c41f20-0000-4000-9000-000000000001"), "آغاز یک زندگی، کنار هم", "با حضور شما شادی ما کامل می‌شود", "wedding", true, "و", "نام عروس", "نام داماد", "wedding", 1, "دعوت‌نامه جشن عروسی", "عروسی" },
                    { new Guid("b7c41f20-0000-4000-9000-000000000002"), "پیوند دو دل", "با حضور شما شادی ما کامل می‌شود", "aghd", true, "و", "نام عروس", "نام داماد", "aghd", 2, "دعوت به مراسم عقد", "عقد" },
                    { new Guid("b7c41f20-0000-4000-9000-000000000003"), "آغاز یک قرار", "به جشن نامزدی ما خوش آمدید", "engagement", true, "و", "نام عروس", "نام داماد", "engagement", 3, "جشن نامزدی و حلقه", "نامزدی" },
                    { new Guid("b7c41f20-0000-4000-9000-000000000004"), "یک بله، یک آغاز", "در این روز خوش کنار ما باشید", "baleboron", true, "و", "نام عروس", "نام داماد", "baleboron", 4, "مراسم بله‌برون", "بله‌برون" },
                    { new Guid("b7c41f20-0000-4000-9000-000000000005"), "شب حنا، شب شادی", "در شب حنابندان منتظر شما هستیم", "hanabandan", true, "و", "نام عروس", "نام داماد", "hanabandan", 5, "شب حنابندان", "حنابندان" },
                    { new Guid("b7c41f20-0000-4000-9000-000000000006"), "سال‌هایی که گذشت", "در جشن سالگرد ما شریک باشید", "anniversary", true, "و", "نام همسر اول", "نام همسر دوم", "anniversary", 6, "سالگرد و تجدید پیمان", "سالگرد ازدواج" },
                    { new Guid("b7c41f20-0000-4000-9000-000000000007"), "یاد او همیشه با ماست", "به مراسم یادبود ایشان دعوت می‌شوید", "memorial", false, "", "نام درگذشته", "", "memorial", 7, "یادبود و مراسم ترحیم", "مجلس ترحیم" },
                    { new Guid("b7c41f20-0000-4000-9000-000000000008"), "یک سال تازه", "به جشن تولد دعوتید", "birthday", false, "", "نام صاحب جشن", "", "birthday", 8, "جشن تولد", "تولد" },
                    { new Guid("b7c41f20-0000-4000-9000-000000000009"), "برای پدر", "روزت مبارک", "fathers-day", false, "", "نام پدر", "", "fathers-day", 9, "تبریک روز پدر", "روز پدر" },
                    { new Guid("b7c41f20-0000-4000-9000-000000000010"), "برای مادر", "روزت مبارک", "mothers-day", false, "", "نام مادر", "", "mothers-day", 10, "تبریک روز مادر", "روز مادر" },
                    { new Guid("b7c41f20-0000-4000-9000-000000000011"), "روز دانشجو", "گرامی باد", "students-day", false, "", "نام مناسبت", "", "students-day", 11, "گرامیداشت روز دانشجو", "روز دانشجو" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Occasions_IsActive_SortOrder",
                schema: "templates",
                table: "Occasions",
                columns: new[] { "IsActive", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Occasions_Slug",
                schema: "templates",
                table: "Occasions",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Occasions",
                schema: "templates");
        }
    }
}
