using Microsoft.EntityFrameworkCore.Migrations;

namespace Tomi.Calendar.Mono.Server.Migrations
{
    public partial class CalendarItemNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserNotes",
                columns: table => new
                {
                    NoteKey = table.Column<int>(type: "integer", nullable: false),
                    UserKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserNotes", x => new { x.NoteKey, x.UserKey });
                    table.ForeignKey(
                        name: "FK_ApplicationUserNotes_AspNetUsers_UserKey",
                        column: x => x.UserKey,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserNotes_Notes_NoteKey",
                        column: x => x.NoteKey,
                        principalTable: "Notes",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserNotes_UserKey",
                table: "ApplicationUserNotes",
                column: "UserKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserNotes");
        }
    }
}
