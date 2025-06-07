using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftYar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditHospitalModel040317 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Hospitals");
        }
    }
}
