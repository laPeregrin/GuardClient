using Microsoft.EntityFrameworkCore.Migrations;

namespace testDAL.Migrations
{
    public partial class _addPermissionTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Permission",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Permission",
                table: "KeyObjects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permission",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Permission",
                table: "KeyObjects");
        }
    }
}
