using Microsoft.EntityFrameworkCore.Migrations;

namespace Tomi.Calendar.Mono.Server.Migrations
{
    public partial class UserCalendarItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserCalendarItem",
                columns: table => new
                {
                    CalendarItemKey = table.Column<int>(type: "integer", nullable: false),
                    UserKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserCalendarItem", x => new { x.CalendarItemKey, x.UserKey });
                    table.ForeignKey(
                        name: "FK_ApplicationUserCalendarItem_AspNetUsers_UserKey",
                        column: x => x.UserKey,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserCalendarItem_CalendarItems_CalendarItemKey",
                        column: x => x.CalendarItemKey,
                        principalTable: "CalendarItems",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCalendarItem_UserKey",
                table: "ApplicationUserCalendarItem",
                column: "UserKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserCalendarItem");
        }
    }
}
