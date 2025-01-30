using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationMicroservice.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig0129 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_Users_UserId",
                table: "UserInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_Permissions_PermissionId",
                table: "UserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PermissionId",
                table: "UserPermissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserInformations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Certifications",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Equipment",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SocialMediaLinks",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PermissionId",
                table: "RolePermissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformations_Users_UserId",
                table: "UserInformations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_Permissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_Users_UserId",
                table: "UserInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_Permissions_PermissionId",
                table: "UserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "Certifications",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "Equipment",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "SocialMediaLinks",
                table: "UserInformations");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PermissionId",
                table: "UserPermissions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserInformations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PermissionId",
                table: "RolePermissions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformations_Users_UserId",
                table: "UserInformations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_Permissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}
