using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftYar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditSpecialtyModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Hospitals_HospitalId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Users_SupervisorId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_Hospitals_HospitalId",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_HospitalId",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_HospitalId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_SupervisorId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "Shifts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HospitalId",
                table: "Specialties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HospitalId",
                table: "Shifts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupervisorId",
                table: "Shifts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_HospitalId",
                table: "Specialties",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_HospitalId",
                table: "Shifts",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_SupervisorId",
                table: "Shifts",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Hospitals_HospitalId",
                table: "Shifts",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Users_SupervisorId",
                table: "Shifts",
                column: "SupervisorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_Hospitals_HospitalId",
                table: "Specialties",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id");
        }
    }
}
