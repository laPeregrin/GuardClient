using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace testDAL.Migrations
{
    public partial class _change_auditoryNumType_to_string : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeyObjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AudNum = table.Column<string>(nullable: true),
                    IsBooked = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeyObjects_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    KeyObjectId = table.Column<Guid>(nullable: false),
                    BookingBegine = table.Column<DateTime>(nullable: false),
                    BookingFinish = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingActions_KeyObjects_KeyObjectId",
                        column: x => x.KeyObjectId,
                        principalTable: "KeyObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingActions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingActions_KeyObjectId",
                table: "BookingActions",
                column: "KeyObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingActions_UserId",
                table: "BookingActions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyObjects_UserId",
                table: "KeyObjects",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingActions");

            migrationBuilder.DropTable(
                name: "KeyObjects");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
