using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftYar.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "UserRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "UserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "UserRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "UserPhoneNumber",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "UserPhoneNumber",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "UserPhoneNumber",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Specialties",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "Specialties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Specialties",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Shifts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "Shifts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Shifts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ShiftRequiredSpecialties",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "ShiftRequiredSpecialties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "ShiftRequiredSpecialties",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ShiftDates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "ShiftDates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "ShiftDates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ShiftAssignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "ShiftAssignments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "ShiftAssignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "RolePermissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "RolePermissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "RefreshTokens",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Permissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "Permissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Permissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "LoginHistories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "LoginHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "LoginHistories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Departments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheUserId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Departments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "UserPhoneNumber");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "UserPhoneNumber");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "UserPhoneNumber");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ShiftRequiredSpecialties");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "ShiftRequiredSpecialties");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "ShiftRequiredSpecialties");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ShiftDates");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "ShiftDates");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "ShiftDates");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ShiftAssignments");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "ShiftAssignments");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "ShiftAssignments");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "LoginHistories");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "LoginHistories");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "LoginHistories");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "TheUserId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Departments");
        }
    }
}
