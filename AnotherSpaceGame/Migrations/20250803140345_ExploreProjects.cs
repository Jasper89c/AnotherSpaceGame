using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Migrations
{
    /// <inheritdoc />
    public partial class ExploreProjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AdvancedExploration",
                table: "UserProjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AdvancedExploration",
                table: "ProjectsResearches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AdvancedExplorationTurnsRequired",
                table: "ProjectsResearches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvancedExploration",
                table: "UserProjects");

            migrationBuilder.DropColumn(
                name: "AdvancedExploration",
                table: "ProjectsResearches");

            migrationBuilder.DropColumn(
                name: "AdvancedExplorationTurnsRequired",
                table: "ProjectsResearches");
        }
    }
}
