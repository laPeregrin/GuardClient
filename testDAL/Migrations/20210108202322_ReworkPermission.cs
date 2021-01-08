using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace testDAL.Migrations
{
    public partial class ReworkPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permission",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Permission",
                table: "KeyObjects");

            migrationBuilder.AddColumn<Guid>(
                name: "PermissionKeyId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    KeyId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_Permissions_KeyObjects_KeyId",
                        column: x => x.KeyId,
                        principalTable: "KeyObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PermissionKeyId",
                table: "Users",
                column: "PermissionKeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Permissions_PermissionKeyId",
                table: "Users",
                column: "PermissionKeyId",
                principalTable: "Permissions",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Permissions_PermissionKeyId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Users_PermissionKeyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PermissionKeyId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Permission",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Permission",
                table: "KeyObjects",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
