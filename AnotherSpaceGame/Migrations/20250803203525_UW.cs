using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Migrations
{
    /// <inheritdoc />
    public partial class UW : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UWHolderId",
                table: "ServerStats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UWHolderName",
                table: "ServerStats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UWWinners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DaysTaken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PowerRating = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPlanets = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UWWinners", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UWWinners");

            migrationBuilder.DropColumn(
                name: "UWHolderId",
                table: "ServerStats");

            migrationBuilder.DropColumn(
                name: "UWHolderName",
                table: "ServerStats");
        }
    }
}
