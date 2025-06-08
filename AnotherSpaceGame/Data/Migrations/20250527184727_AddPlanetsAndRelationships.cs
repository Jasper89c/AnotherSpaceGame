using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanetsAndRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanetId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AvailableOre = table.Column<int>(type: "int", nullable: false),
                    MineralProduced = table.Column<int>(type: "int", nullable: false),
                    TotalLand = table.Column<int>(type: "int", nullable: false),
                    FoodRequired = table.Column<int>(type: "int", nullable: false),
                    GoodsRequired = table.Column<int>(type: "int", nullable: false),
                    CurrentPopulation = table.Column<int>(type: "int", nullable: false),
                    MaxPopulation = table.Column<int>(type: "int", nullable: false),
                    Loyalty = table.Column<int>(type: "int", nullable: false),
                    AvailableLabour = table.Column<int>(type: "int", nullable: false),
                    LandAvailable = table.Column<int>(type: "int", nullable: false),
                    Housing = table.Column<int>(type: "int", nullable: false),
                    Commercial = table.Column<int>(type: "int", nullable: false),
                    Industry = table.Column<int>(type: "int", nullable: false),
                    Agriculture = table.Column<int>(type: "int", nullable: false),
                    Mining = table.Column<int>(type: "int", nullable: false),
                    ArtifactModifier = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Planets_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Planets_ApplicationUserId",
                table: "Planets",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Planets");
        }
    }
}
