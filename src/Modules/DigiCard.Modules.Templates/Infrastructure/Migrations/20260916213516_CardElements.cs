using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiCard.Modules.Templates.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CardElements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Elements",
                schema: "templates",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Elements",
                schema: "templates",
                table: "Templates");
        }
    }
}
