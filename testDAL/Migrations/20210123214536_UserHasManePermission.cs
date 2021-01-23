using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace testDAL.Migrations
{
    public partial class UserHasManePermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_KeyObjects_KeyId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Permissions_PermissionKeyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PermissionKeyId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "PermissionKeyId",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "KeyId",
                table: "Permissions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PermissionUser",
                columns: table => new
                {
                    PermissionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersWithPermissionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionUser", x => new { x.PermissionsId, x.UsersWithPermissionsId });
                    table.ForeignKey(
                        name: "FK_PermissionUser_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionUser_Users_UsersWithPermissionsId",
                        column: x => x.UsersWithPermissionsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_KeyId",
                table: "Permissions",
                column: "KeyId",
                unique: true,
                filter: "[KeyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionUser_UsersWithPermissionsId",
                table: "PermissionUser",
                column: "UsersWithPermissionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_KeyObjects_KeyId",
                table: "Permissions",
                column: "KeyId",
                principalTable: "KeyObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_KeyObjects_KeyId",
                table: "Permissions");

            migrationBuilder.DropTable(
                name: "PermissionUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_KeyId",
                table: "Permissions");

            migrationBuilder.AddColumn<Guid>(
                name: "PermissionKeyId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "KeyId",
                table: "Permissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "KeyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PermissionKeyId",
                table: "Users",
                column: "PermissionKeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_KeyObjects_KeyId",
                table: "Permissions",
                column: "KeyId",
                principalTable: "KeyObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Permissions_PermissionKeyId",
                table: "Users",
                column: "PermissionKeyId",
                principalTable: "Permissions",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
