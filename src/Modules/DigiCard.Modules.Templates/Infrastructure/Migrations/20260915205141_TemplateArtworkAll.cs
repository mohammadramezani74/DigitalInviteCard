using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiCard.Modules.Templates.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TemplateArtworkAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0101"),
                columns: new[] { "Accent", "Artwork", "Frame", "Ink", "Layout", "SafeBottom", "SafeTop", "Typeface" },
                values: new object[] { "#918063", "bagh-e-irani", "none", "#3c3a36", "centered", 67, 31, "serif" });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0102"),
                columns: new[] { "Accent", "Artwork", "Frame", "Ink", "Layout", "SafeBottom", "SafeTop" },
                values: new object[] { "#8a7c5c", "roz-o-morvarid", "none", "#3c3a36", "centered", 62, 26 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0103"),
                columns: new[] { "Accent", "Artwork", "Frame", "Ink", "SafeBottom", "SafeTop", "Version" },
                values: new object[] { "#847b57", "sepid-sade", "none", "#3c3a36", 80, 44, 2 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0104"),
                columns: new[] { "Accent", "Artwork", "Ink", "Layout", "SafeBottom", "SafeTop", "Typeface", "Version" },
                values: new object[] { "#7c764f", "khat-e-noor", "#3c3a36", "centered", 58, 22, "display", 2 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0105"),
                columns: new[] { "Accent", "Artwork", "Frame", "Ink", "SafeBottom", "SafeTop", "Version" },
                values: new object[] { "#807547", "sadegi-talaei", "none", "#3c3a36", 58, 22, 2 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0107"),
                columns: new[] { "Accent", "Artwork", "Ink", "SafeBottom", "SafeTop", "Version" },
                values: new object[] { "#85785d", "shaghayegh", "#3c3a36", 76, 40, 2 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0108"),
                columns: new[] { "Accent", "Artwork", "Ink", "Layout", "SafeBottom", "SafeTop", "Typeface", "Version" },
                values: new object[] { "#6d5c7f", "banafsheh", "#3c3a36", "centered", 81, 45, "serif", 2 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0109"),
                columns: new[] { "Accent", "Artwork", "Ink", "Layout", "SafeBottom", "SafeTop", "Typeface", "Version" },
                values: new object[] { "#80673d", "hana-o-gol", "#3c3a36", "centered", 50, 14, "serif", 2 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010b"),
                columns: new[] { "Accent", "Artwork", "Frame", "Ink", "SafeBottom", "SafeTop", "Typeface", "Version" },
                values: new object[] { "#566c84", "kashi-firouzeh", "none", "#3c3a36", 70, 34, "display", 2 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010c"),
                columns: new[] { "Accent", "Artwork", "Frame", "Ink", "Layout", "SafeBottom", "SafeTop", "Typeface", "Version" },
                values: new object[] { "#7d6143", "ghab-e-khatereh", "none", "#3c3a36", "centered", 69, 33, "serif", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0101"),
                columns: new[] { "Accent", "Artwork", "Frame", "Ink", "Layout", "SafeBottom", "SafeTop", "Typeface" },
                values: new object[] { "#52796f", "", "ornate", "#2b3b35", "arch", 75, 30, "display" });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0102"),
                columns: new[] { "Accent", "Artwork", "Frame", "Ink", "Layout", "SafeBottom", "SafeTop" },
                values: new object[] { "#a56b7d", "", "thin", "#3d2f34", "photo", 75, 30 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0103"),
                columns: new[] { "Accent", "Artwork", "Frame", "Ink", "SafeBottom", "SafeTop", "Version" },
                values: new object[] { "#8a7f6b", "", "thin", "#3b3a35", 75, 30, 1 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0104"),
                columns: new[] { "Accent", "Artwork", "Ink", "Layout", "SafeBottom", "SafeTop", "Typeface", "Version" },
                values: new object[] { "#9a8c74", "", "#35332e", "banded", 75, 30, "sans", 1 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0105"),
                columns: new[] { "Accent", "Artwork", "Frame", "Ink", "SafeBottom", "SafeTop", "Version" },
                values: new object[] { "#b08d57", "", "thin", "#37352f", 75, 30, 1 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0107"),
                columns: new[] { "Accent", "Artwork", "Ink", "SafeBottom", "SafeTop", "Version" },
                values: new object[] { "#c4636b", "", "#46333a", 75, 30, 1 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0108"),
                columns: new[] { "Accent", "Artwork", "Ink", "Layout", "SafeBottom", "SafeTop", "Typeface", "Version" },
                values: new object[] { "#7d6ea8", "", "#3b3548", "arch", 75, 30, "display", 1 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0109"),
                columns: new[] { "Accent", "Artwork", "Ink", "Layout", "SafeBottom", "SafeTop", "Typeface", "Version" },
                values: new object[] { "#c98a3e", "", "#45362a", "banded", 75, 30, "display", 1 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010b"),
                columns: new[] { "Accent", "Artwork", "Frame", "Ink", "SafeBottom", "SafeTop", "Typeface", "Version" },
                values: new object[] { "#2e7d8c", "", "ornate", "#23383d", 75, 30, "serif", 1 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010c"),
                columns: new[] { "Accent", "Artwork", "Frame", "Ink", "Layout", "SafeBottom", "SafeTop", "Typeface", "Version" },
                values: new object[] { "#6b6f7d", "", "double", "#2f3138", "photo", 75, 30, "sans", 1 });
        }
    }
}
