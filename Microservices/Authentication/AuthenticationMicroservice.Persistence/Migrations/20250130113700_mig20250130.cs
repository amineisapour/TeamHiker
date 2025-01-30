using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationMicroservice.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig20250130 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_Users_UserId",
                table: "UserInformations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInformations",
                table: "UserInformations");

            migrationBuilder.DropIndex(
                name: "IX_UserInformations_UserId",
                table: "UserInformations");

            migrationBuilder.RenameTable(
                name: "UserInformations",
                newName: "UserInformation");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "UserInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserInformation",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "UserInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInformation",
                table: "UserInformation",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserInformation_UserId",
                table: "UserInformation",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformation_Users_UserId",
                table: "UserInformation",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInformation_Users_UserId",
                table: "UserInformation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInformation",
                table: "UserInformation");

            migrationBuilder.DropIndex(
                name: "IX_UserInformation_UserId",
                table: "UserInformation");

            migrationBuilder.RenameTable(
                name: "UserInformation",
                newName: "UserInformations");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Roles",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Permissions",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserInformations",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInformations",
                table: "UserInformations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_UserId",
                table: "UserInformations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformations_Users_UserId",
                table: "UserInformations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
