using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCommoditiesAndUserLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commodities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    Food = table.Column<int>(type: "int", nullable: false),
                    Ore = table.Column<int>(type: "int", nullable: false),
                    RawMaterial = table.Column<int>(type: "int", nullable: false),
                    ConsumerGoods = table.Column<int>(type: "int", nullable: false),
                    TerranMetal = table.Column<int>(type: "int", nullable: false),
                    RedCrystal = table.Column<int>(type: "int", nullable: false),
                    WhiteCrystal = table.Column<int>(type: "int", nullable: false),
                    Rutile = table.Column<int>(type: "int", nullable: false),
                    Composite = table.Column<int>(type: "int", nullable: false),
                    StrafezOrganism = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commodities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commodities_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commodities_ApplicationUserId",
                table: "Commodities",
                column: "ApplicationUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commodities");
        }
    }
}
