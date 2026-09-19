using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiCard.Modules.Templates.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TemplateArtwork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Artwork",
                schema: "templates",
                table: "Templates",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SafeBottom",
                schema: "templates",
                table: "Templates",
                type: "int",
                nullable: false,
                defaultValue: 75);

            migrationBuilder.AddColumn<int>(
                name: "SafeTop",
                schema: "templates",
                table: "Templates",
                type: "int",
                nullable: false,
                defaultValue: 30);

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0101"),
                columns: new[] { "Artwork", "SafeBottom", "SafeTop" },
                values: new object[] { "", 75, 30 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0102"),
                columns: new[] { "Artwork", "SafeBottom", "SafeTop" },
                values: new object[] { "", 75, 30 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0103"),
                columns: new[] { "Artwork", "SafeBottom", "SafeTop" },
                values: new object[] { "", 75, 30 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0104"),
                columns: new[] { "Artwork", "SafeBottom", "SafeTop" },
                values: new object[] { "", 75, 30 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0105"),
                columns: new[] { "Artwork", "SafeBottom", "SafeTop" },
                values: new object[] { "", 75, 30 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0106"),
                columns: new[] { "Artwork", "Layout", "SafeBottom", "SafeTop", "Typeface" },
                values: new object[] { "golab-abrang", "centered", 67, 37, "serif" });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0107"),
                columns: new[] { "Artwork", "SafeBottom", "SafeTop" },
                values: new object[] { "", 75, 30 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0108"),
                columns: new[] { "Artwork", "SafeBottom", "SafeTop" },
                values: new object[] { "", 75, 30 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0109"),
                columns: new[] { "Artwork", "SafeBottom", "SafeTop" },
                values: new object[] { "", 75, 30 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010a"),
                columns: new[] { "Artwork", "SafeBottom", "SafeTop" },
                values: new object[] { "", 75, 30 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010b"),
                columns: new[] { "Artwork", "SafeBottom", "SafeTop" },
                values: new object[] { "", 75, 30 });

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b010c"),
                columns: new[] { "Artwork", "SafeBottom", "SafeTop" },
                values: new object[] { "", 75, 30 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Artwork",
                schema: "templates",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "SafeBottom",
                schema: "templates",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "SafeTop",
                schema: "templates",
                table: "Templates");

            migrationBuilder.UpdateData(
                schema: "templates",
                table: "Templates",
                keyColumn: "Id",
                keyValue: new Guid("a41a6319-3438-4ef9-89da-fae6b30b0106"),
                columns: new[] { "Layout", "Typeface" },
                values: new object[] { "arch", "display" });
        }
    }
}
