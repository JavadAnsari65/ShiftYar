using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftYar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddShiftDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShiftDate",
                table: "ShiftAssignments");

            migrationBuilder.AddColumn<int>(
                name: "ShiftDateId",
                table: "ShiftAssignments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShiftDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersianDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsHoliday = table.Column<bool>(type: "bit", nullable: true),
                    HolidayTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftDates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignments_ShiftDateId",
                table: "ShiftAssignments",
                column: "ShiftDateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_ShiftDates_ShiftDateId",
                table: "ShiftAssignments",
                column: "ShiftDateId",
                principalTable: "ShiftDates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssignments_ShiftDates_ShiftDateId",
                table: "ShiftAssignments");

            migrationBuilder.DropTable(
                name: "ShiftDates");

            migrationBuilder.DropIndex(
                name: "IX_ShiftAssignments_ShiftDateId",
                table: "ShiftAssignments");

            migrationBuilder.DropColumn(
                name: "ShiftDateId",
                table: "ShiftAssignments");

            migrationBuilder.AddColumn<DateTime>(
                name: "ShiftDate",
                table: "ShiftAssignments",
                type: "datetime2",
                nullable: true);
        }
    }
}
