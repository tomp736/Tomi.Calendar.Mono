using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace Tomi.Calendar.Mono.Server.Migrations
{
    public partial class UseNodaTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<LocalDate>(
                name: "StartDate",
                table: "CalendarItems",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<LocalDate>(
                name: "EndDate",
                table: "CalendarItems",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<LocalTime>(
                name: "EndTime",
                table: "CalendarItems",
                type: "time",
                nullable: false,
                defaultValue: new NodaTime.LocalTime(0, 0));

            migrationBuilder.AddColumn<LocalTime>(
                name: "StartTime",
                table: "CalendarItems",
                type: "time",
                nullable: false,
                defaultValue: new NodaTime.LocalTime(0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "CalendarItems");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "CalendarItems");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "CalendarItems",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(LocalDate),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "CalendarItems",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(LocalDate),
                oldType: "date");
        }
    }
}
