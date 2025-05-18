using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftYar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupervisorId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SupervisorId",
                table: "Departments",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_SupervisorId",
                table: "Departments",
                column: "SupervisorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_SupervisorId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_SupervisorId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "Departments");
        }
    }
}
