using Microsoft.EntityFrameworkCore.Migrations;

namespace Tomi.Calendar.Mono.Server.Migrations
{
    public partial class AddNotes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarItemNote_CalendarItems_CalendarItemKey",
                table: "CalendarItemNote");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarItemNote_Note_NoteKey",
                table: "CalendarItemNote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Note",
                table: "Note");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalendarItemNote",
                table: "CalendarItemNote");

            migrationBuilder.RenameTable(
                name: "Note",
                newName: "Notes");

            migrationBuilder.RenameTable(
                name: "CalendarItemNote",
                newName: "CalendarItemNotes");

            migrationBuilder.RenameIndex(
                name: "IX_CalendarItemNote_NoteKey",
                table: "CalendarItemNotes",
                newName: "IX_CalendarItemNotes_NoteKey");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notes",
                table: "Notes",
                column: "Key");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalendarItemNotes",
                table: "CalendarItemNotes",
                columns: new[] { "CalendarItemKey", "NoteKey" });

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarItemNotes_CalendarItems_CalendarItemKey",
                table: "CalendarItemNotes",
                column: "CalendarItemKey",
                principalTable: "CalendarItems",
                principalColumn: "Key",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarItemNotes_Notes_NoteKey",
                table: "CalendarItemNotes",
                column: "NoteKey",
                principalTable: "Notes",
                principalColumn: "Key",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarItemNotes_CalendarItems_CalendarItemKey",
                table: "CalendarItemNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarItemNotes_Notes_NoteKey",
                table: "CalendarItemNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notes",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalendarItemNotes",
                table: "CalendarItemNotes");

            migrationBuilder.RenameTable(
                name: "Notes",
                newName: "Note");

            migrationBuilder.RenameTable(
                name: "CalendarItemNotes",
                newName: "CalendarItemNote");

            migrationBuilder.RenameIndex(
                name: "IX_CalendarItemNotes_NoteKey",
                table: "CalendarItemNote",
                newName: "IX_CalendarItemNote_NoteKey");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Note",
                table: "Note",
                column: "Key");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalendarItemNote",
                table: "CalendarItemNote",
                columns: new[] { "CalendarItemKey", "NoteKey" });

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarItemNote_CalendarItems_CalendarItemKey",
                table: "CalendarItemNote",
                column: "CalendarItemKey",
                principalTable: "CalendarItems",
                principalColumn: "Key",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarItemNote_Note_NoteKey",
                table: "CalendarItemNote",
                column: "NoteKey",
                principalTable: "Note",
                principalColumn: "Key",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
