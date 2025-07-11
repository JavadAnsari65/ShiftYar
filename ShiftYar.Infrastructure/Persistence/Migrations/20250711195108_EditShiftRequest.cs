using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftYar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditShiftRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftRequests_ShiftDates_ShiftDateId",
                table: "ShiftRequests");

            migrationBuilder.DropIndex(
                name: "IX_ShiftRequests_ShiftDateId",
                table: "ShiftRequests");

            migrationBuilder.DropColumn(
                name: "ShiftDateId",
                table: "ShiftRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShiftDateId",
                table: "ShiftRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShiftRequests_ShiftDateId",
                table: "ShiftRequests",
                column: "ShiftDateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftRequests_ShiftDates_ShiftDateId",
                table: "ShiftRequests",
                column: "ShiftDateId",
                principalTable: "ShiftDates",
                principalColumn: "Id");
        }
    }
}
