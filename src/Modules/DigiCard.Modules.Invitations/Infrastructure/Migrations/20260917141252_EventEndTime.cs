using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiCard.Modules.Invitations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EventEndTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "EventEndTime",
                schema: "invitations",
                table: "Drafts",
                type: "time(0)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventEndTime",
                schema: "invitations",
                table: "Drafts");
        }
    }
}
