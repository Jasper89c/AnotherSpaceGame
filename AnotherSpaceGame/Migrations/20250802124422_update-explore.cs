using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Migrations
{
    /// <inheritdoc />
    public partial class updateexplore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ExplorationPointsNeeded",
                table: "Explorations",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ExplorationPointsNeeded",
                table: "Explorations",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
