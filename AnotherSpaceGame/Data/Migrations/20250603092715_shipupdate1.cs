using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class shipupdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VClassCruiser1TurnsRequired",
                table: "ViralResearches",
                newName: "VClassCruiserTurnsRequired");

            migrationBuilder.AddColumn<int>(
                name: "BuildRate",
                table: "Ships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "FighterClass",
                table: "CollectiveResearches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "FighterClassTurnsRequired",
                table: "CollectiveResearches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RClassCorvette",
                table: "CollectiveResearches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RClassCorvetteTurnsRequired",
                table: "CollectiveResearches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildRate",
                table: "Ships");

            migrationBuilder.DropColumn(
                name: "FighterClass",
                table: "CollectiveResearches");

            migrationBuilder.DropColumn(
                name: "FighterClassTurnsRequired",
                table: "CollectiveResearches");

            migrationBuilder.DropColumn(
                name: "RClassCorvette",
                table: "CollectiveResearches");

            migrationBuilder.DropColumn(
                name: "RClassCorvetteTurnsRequired",
                table: "CollectiveResearches");

            migrationBuilder.RenameColumn(
                name: "VClassCruiserTurnsRequired",
                table: "ViralResearches",
                newName: "VClassCruiser1TurnsRequired");
        }
    }
}
