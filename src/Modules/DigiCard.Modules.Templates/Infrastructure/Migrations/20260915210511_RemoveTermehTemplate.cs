using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiCard.Modules.Templates.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTermehTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010a"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "templates",
                table: "Templates",
                columns: new[] { "Id", "Accent", "Artwork", "Background", "Category", "Family", "Frame", "Ink", "Layout", "Name", "Ornament", "SafeBottom", "SafeTop", "Slug", "Typeface", "Version" },
                values: new object[] { new Guid("a41a6319-3438-4ef9-89da-fae6b30b010a"), "#8c3b3b", "", "linear-gradient(180deg,#fbf3ee 0%,#f3e4dc 100%)", "aghd", "persian", "ornate", "#3a2424", "banded", "ترمه", "paisley", 75, 30, "termeh", "display", 1 });
        }
    }
}
