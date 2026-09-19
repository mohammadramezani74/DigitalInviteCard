using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigiCard.Modules.Templates.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TemplateDesignTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Background",
                schema: "templates",
                table: "Templates",
                type: "nvarchar(240)",
                maxLength: 240,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                schema: "templates",
                table: "Templates",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Family",
                schema: "templates",
                table: "Templates",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Frame",
                schema: "templates",
                table: "Templates",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ink",
                schema: "templates",
                table: "Templates",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Layout",
                schema: "templates",
                table: "Templates",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ornament",
                schema: "templates",
                table: "Templates",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                schema: "templates",
                table: "Templates",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Typeface",
                schema: "templates",
                table: "Templates",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0101"),
                columns: new[] { "Background", "Category", "Family", "Frame", "Ink", "Layout", "Ornament", "Slug", "Typeface", "Version" },
                values: new object[] { "linear-gradient(180deg,#f4f7f3 0%,#e9f0ea 100%)", "wedding", "persian", "ornate", "#2b3b35", "arch", "paisley", "bagh-e-irani", "display", 2 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0102"),
                columns: new[] { "Background", "Category", "Family", "Frame", "Ink", "Layout", "Ornament", "Slug", "Typeface", "Version" },
                values: new object[] { "linear-gradient(180deg,#fbf4f6 0%,#f2e6ea 100%)", "wedding", "photo", "thin", "#3d2f34", "photo", "dot", "roz-o-morvarid", "serif", 2 });

            migrationBuilder.InsertData(
                schema: "templates",
                table: "Templates",
                columns: new[] { "Id", "Accent", "Background", "Category", "Family", "Frame", "Ink", "Layout", "Name", "Ornament", "Slug", "Typeface", "Version" },
                values: new object[,]
                {
                    { new Guid("a41a6319-3438-4ef9-89da-fae6b30b0103"), "#8a7f6b", "linear-gradient(180deg,#fffdf9 0%,#f6f1e8 100%)", "wedding", "minimal", "thin", "#3b3a35", "centered", "سپید ساده", "rule", "sepid-sade", "serif", 1 },
                    { new Guid("a41a6319-3438-4ef9-89da-fae6b30b0104"), "#9a8c74", "#fbfaf7", "aghd", "minimal", "none", "#35332e", "banded", "خط نور", "dot", "khat-e-noor", "sans", 1 },
                    { new Guid("a41a6319-3438-4ef9-89da-fae6b30b0105"), "#b08d57", "linear-gradient(180deg,#fdfbf6 0%,#f4ecdd 100%)", "anniversary", "minimal", "thin", "#37352f", "centered", "سادگی طلایی", "rule", "sadegi-talaei", "display", 1 },
                    { new Guid("a41a6319-3438-4ef9-89da-fae6b30b0106"), "#c17f8f", "radial-gradient(120% 80% at 50% 0%,#fdeef1 0%,#fbf7f4 55%,#f7eee9 100%)", "wedding", "watercolor", "none", "#4a3a3e", "arch", "گلاب آبرنگ", "floral", "golab-abrang", "display", 1 },
                    { new Guid("a41a6319-3438-4ef9-89da-fae6b30b0107"), "#c4636b", "radial-gradient(100% 70% at 20% 10%,#fdeceb 0%,#fcf8f5 60%,#f8f1ec 100%)", "engagement", "watercolor", "none", "#46333a", "centered", "شقایق", "floral", "shaghayegh", "serif", 1 },
                    { new Guid("a41a6319-3438-4ef9-89da-fae6b30b0108"), "#7d6ea8", "radial-gradient(110% 80% at 80% 0%,#f1edfa 0%,#faf8fc 55%,#f4f1f7 100%)", "baleboron", "watercolor", "none", "#3b3548", "arch", "بنفشه", "floral", "banafsheh", "display", 1 },
                    { new Guid("a41a6319-3438-4ef9-89da-fae6b30b0109"), "#c98a3e", "linear-gradient(160deg,#fdf4e4 0%,#fbf6ee 55%,#f6ead6 100%)", "hanabandan", "watercolor", "none", "#45362a", "banded", "حنا و گل", "floral", "hana-o-gol", "display", 1 },
                    { new Guid("a41a6319-3438-4ef9-89da-fae6b30b010a"), "#8c3b3b", "linear-gradient(180deg,#fbf3ee 0%,#f3e4dc 100%)", "aghd", "persian", "ornate", "#3a2424", "banded", "ترمه", "paisley", "termeh", "display", 1 },
                    { new Guid("a41a6319-3438-4ef9-89da-fae6b30b010b"), "#2e7d8c", "linear-gradient(180deg,#eff7f8 0%,#e2eff1 100%)", "hanabandan", "persian", "ornate", "#23383d", "centered", "کاشی فیروزه", "geometric", "kashi-firouzeh", "serif", 1 },
                    { new Guid("a41a6319-3438-4ef9-89da-fae6b30b010c"), "#6b6f7d", "linear-gradient(180deg,#f6f7f9 0%,#e9ebef 100%)", "anniversary", "photo", "double", "#2f3138", "photo", "قاب خاطره", "rule", "ghab-e-khatereh", "sans", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Templates_Category",
                schema: "templates",
                table: "Templates",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_Slug",
                schema: "templates",
                table: "Templates",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Templates_Category",
                schema: "templates",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_Slug",
                schema: "templates",
                table: "Templates");

            migrationBuilder.DeleteData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0103"));

            migrationBuilder.DeleteData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0104"));

            migrationBuilder.DeleteData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0105"));

            migrationBuilder.DeleteData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0106"));

            migrationBuilder.DeleteData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0107"));

            migrationBuilder.DeleteData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0108"));

            migrationBuilder.DeleteData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0109"));

            migrationBuilder.DeleteData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010a"));

            migrationBuilder.DeleteData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010b"));

            migrationBuilder.DeleteData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010c"));

            migrationBuilder.DropColumn(
                name: "Background",
                schema: "templates",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "Category",
                schema: "templates",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "Family",
                schema: "templates",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "Frame",
                schema: "templates",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "Ink",
                schema: "templates",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "Layout",
                schema: "templates",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "Ornament",
                schema: "templates",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "Slug",
                schema: "templates",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "Typeface",
                schema: "templates",
                table: "Templates");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0101"),
                column: "Version",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0102"),
                column: "Version",
                value: 1);
        }
    }
}
