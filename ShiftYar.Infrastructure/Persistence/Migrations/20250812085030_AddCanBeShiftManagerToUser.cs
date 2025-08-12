using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftYar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCanBeShiftManagerToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanBeShiftManager",
                table: "Users",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanBeShiftManager",
                table: "Users");
        }
    }
}
