using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Tomi.Calendar.Mono.Server.Migrations
{
    public partial class RemoveNotesTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserNotes");

            migrationBuilder.DropTable(
                name: "ApplicationUserTags");

            migrationBuilder.DropTable(
                name: "CalendarItemNotes");

            migrationBuilder.DropTable(
                name: "CalendarItemTags");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Tags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Key = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Key = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Color = table.Column<string>(type: "text", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Key);
                });

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

            migrationBuilder.CreateTable(
                name: "CalendarItemNotes",
                columns: table => new
                {
                    CalendarItemKey = table.Column<int>(type: "integer", nullable: false),
                    NoteKey = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarItemNotes", x => new { x.CalendarItemKey, x.NoteKey });
                    table.ForeignKey(
                        name: "FK_CalendarItemNotes_CalendarItems_CalendarItemKey",
                        column: x => x.CalendarItemKey,
                        principalTable: "CalendarItems",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalendarItemNotes_Notes_NoteKey",
                        column: x => x.NoteKey,
                        principalTable: "Notes",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserTags",
                columns: table => new
                {
                    TagKey = table.Column<int>(type: "integer", nullable: false),
                    UserKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserTags", x => new { x.TagKey, x.UserKey });
                    table.ForeignKey(
                        name: "FK_ApplicationUserTags_AspNetUsers_UserKey",
                        column: x => x.UserKey,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserTags_Tags_TagKey",
                        column: x => x.TagKey,
                        principalTable: "Tags",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalendarItemTags",
                columns: table => new
                {
                    CalendarItemKey = table.Column<int>(type: "integer", nullable: false),
                    TagKey = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarItemTags", x => new { x.CalendarItemKey, x.TagKey });
                    table.ForeignKey(
                        name: "FK_CalendarItemTags_CalendarItems_CalendarItemKey",
                        column: x => x.CalendarItemKey,
                        principalTable: "CalendarItems",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalendarItemTags_Tags_TagKey",
                        column: x => x.TagKey,
                        principalTable: "Tags",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserNotes_UserKey",
                table: "ApplicationUserNotes",
                column: "UserKey");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTags_UserKey",
                table: "ApplicationUserTags",
                column: "UserKey");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarItemNotes_NoteKey",
                table: "CalendarItemNotes",
                column: "NoteKey");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarItemTags_TagKey",
                table: "CalendarItemTags",
                column: "TagKey");
        }
    }
}
