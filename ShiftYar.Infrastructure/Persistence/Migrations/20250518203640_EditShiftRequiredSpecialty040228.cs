using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftYar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditShiftRequiredSpecialty040228 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequiredAnyGenderCount",
                table: "ShiftRequiredSpecialties",
                newName: "RequiredTottalCount");

            migrationBuilder.RenameColumn(
                name: "OnCallAnyGenderCount",
                table: "ShiftRequiredSpecialties",
                newName: "OnCallTottalCount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequiredTottalCount",
                table: "ShiftRequiredSpecialties",
                newName: "RequiredAnyGenderCount");

            migrationBuilder.RenameColumn(
                name: "OnCallTottalCount",
                table: "ShiftRequiredSpecialties",
                newName: "OnCallAnyGenderCount");
        }
    }
}
