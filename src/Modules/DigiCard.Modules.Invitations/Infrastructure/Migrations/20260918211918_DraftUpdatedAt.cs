using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiCard.Modules.Invitations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DraftUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Drafts_OwnerId_CreatedAt",
                schema: "invitations",
                table: "Drafts");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                schema: "invitations",
                table: "Drafts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_Drafts_OwnerId_UpdatedAt",
                schema: "invitations",
                table: "Drafts",
                columns: new[] { "OwnerId", "UpdatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Drafts_OwnerId_UpdatedAt",
                schema: "invitations",
                table: "Drafts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "invitations",
                table: "Drafts");

            migrationBuilder.CreateIndex(
                name: "IX_Drafts_OwnerId_CreatedAt",
                schema: "invitations",
                table: "Drafts",
                columns: new[] { "OwnerId", "CreatedAt" });
        }
    }
}
