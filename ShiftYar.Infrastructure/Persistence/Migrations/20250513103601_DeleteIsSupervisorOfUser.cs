using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftYar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeleteIsSupervisorOfUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSupervisor",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSupervisor",
                table: "Users",
                type: "bit",
                nullable: true);
        }
    }
}
