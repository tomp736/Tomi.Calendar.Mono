using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Tomi.Calendar.Mono.Server.Migrations
{
    public partial class CalendarItemTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CalendarItems",
                table: "CalendarItems");

            migrationBuilder.AddColumn<int>(
                name: "Key",
                table: "CalendarItems",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalendarItems",
                table: "CalendarItems",
                column: "Key");

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Key = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "CalendarItemTags",
                columns: table => new
                {
                    CalendarItemKey = table.Column<int>(type: "integer", nullable: false),
                    TagKey = table.Column<int>(type: "integer", nullable: false),
                    Key = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
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
                name: "IX_CalendarItemTags_TagKey",
                table: "CalendarItemTags",
                column: "TagKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarItemTags");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalendarItems",
                table: "CalendarItems");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "CalendarItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalendarItems",
                table: "CalendarItems",
                column: "Id");
        }
    }
}
