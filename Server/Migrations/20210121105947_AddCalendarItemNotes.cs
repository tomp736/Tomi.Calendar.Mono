using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Tomi.Calendar.Mono.Server.Migrations
{
    public partial class AddCalendarItemNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    Key = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "CalendarItemNote",
                columns: table => new
                {
                    CalendarItemKey = table.Column<int>(type: "integer", nullable: false),
                    NoteKey = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarItemNote", x => new { x.CalendarItemKey, x.NoteKey });
                    table.ForeignKey(
                        name: "FK_CalendarItemNote_CalendarItems_CalendarItemKey",
                        column: x => x.CalendarItemKey,
                        principalTable: "CalendarItems",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalendarItemNote_Note_NoteKey",
                        column: x => x.NoteKey,
                        principalTable: "Note",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarItemNote_NoteKey",
                table: "CalendarItemNote",
                column: "NoteKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarItemNote");

            migrationBuilder.DropTable(
                name: "Note");
        }
    }
}
