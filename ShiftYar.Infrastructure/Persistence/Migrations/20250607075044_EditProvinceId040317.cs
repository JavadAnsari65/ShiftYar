using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftYar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditProvinceId040317 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Provinces_Provinceid",
                table: "Cities");

            migrationBuilder.RenameColumn(
                name: "Provinceid",
                table: "Cities",
                newName: "ProvinceId");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_Provinceid",
                table: "Cities",
                newName: "IX_Cities_ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Provinces_ProvinceId",
                table: "Cities",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Provinces_ProvinceId",
                table: "Cities");

            migrationBuilder.RenameColumn(
                name: "ProvinceId",
                table: "Cities",
                newName: "Provinceid");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_ProvinceId",
                table: "Cities",
                newName: "IX_Cities_Provinceid");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Provinces_Provinceid",
                table: "Cities",
                column: "Provinceid",
                principalTable: "Provinces",
                principalColumn: "Id");
        }
    }
}
