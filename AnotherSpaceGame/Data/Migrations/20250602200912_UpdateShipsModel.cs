using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShipsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScanningPower",
                table: "Ships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "ArtifactModifier",
                table: "Planets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "AgricultureModifier",
                table: "Planets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OreModifier",
                table: "Planets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PopulationModifier",
                table: "Planets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PowerRating",
                table: "Planets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPlanets",
                table: "Planets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalUpkeep",
                table: "Fleets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExplorationCompleted",
                table: "Explorations",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FederationsId",
                table: "Battlelogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ArtifactShield",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "FederationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Federations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FederationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FederationLeaderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalMembers = table.Column<int>(type: "int", nullable: false),
                    MaximumMembers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Federations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Federations_AspNetUsers_FederationLeaderId",
                        column: x => x.FederationLeaderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FederationMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FederationsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FederationMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FederationMessages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FederationMessages_Federations_FederationsId",
                        column: x => x.FederationsId,
                        principalTable: "Federations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FederationsFederations",
                columns: table => new
                {
                    FederationWarsId = table.Column<int>(type: "int", nullable: false),
                    FederationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FederationsFederations", x => new { x.FederationWarsId, x.FederationsId });
                    table.ForeignKey(
                        name: "FK_FederationsFederations_Federations_FederationWarsId",
                        column: x => x.FederationWarsId,
                        principalTable: "Federations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FederationsFederations_Federations_FederationsId",
                        column: x => x.FederationsId,
                        principalTable: "Federations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Battlelogs_FederationsId",
                table: "Battlelogs",
                column: "FederationsId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FederationId",
                table: "AspNetUsers",
                column: "FederationId");

            migrationBuilder.CreateIndex(
                name: "IX_FederationMessages_FederationsId",
                table: "FederationMessages",
                column: "FederationsId");

            migrationBuilder.CreateIndex(
                name: "IX_FederationMessages_SenderId",
                table: "FederationMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Federations_FederationLeaderId",
                table: "Federations",
                column: "FederationLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_FederationsFederations_FederationsId",
                table: "FederationsFederations",
                column: "FederationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Federations_FederationId",
                table: "AspNetUsers",
                column: "FederationId",
                principalTable: "Federations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Battlelogs_Federations_FederationsId",
                table: "Battlelogs",
                column: "FederationsId",
                principalTable: "Federations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Federations_FederationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Battlelogs_Federations_FederationsId",
                table: "Battlelogs");

            migrationBuilder.DropTable(
                name: "FederationMessages");

            migrationBuilder.DropTable(
                name: "FederationsFederations");

            migrationBuilder.DropTable(
                name: "Federations");

            migrationBuilder.DropIndex(
                name: "IX_Battlelogs_FederationsId",
                table: "Battlelogs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FederationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ScanningPower",
                table: "Ships");

            migrationBuilder.DropColumn(
                name: "AgricultureModifier",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "OreModifier",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "PopulationModifier",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "PowerRating",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "TotalPlanets",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "TotalUpkeep",
                table: "Fleets");

            migrationBuilder.DropColumn(
                name: "FederationsId",
                table: "Battlelogs");

            migrationBuilder.DropColumn(
                name: "ArtifactShield",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FederationId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ArtifactModifier",
                table: "Planets",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "ExplorationCompleted",
                table: "Explorations",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
