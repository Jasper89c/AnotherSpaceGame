using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class CEResearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BattlesLost",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BattlesWon",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ColoniesExplored",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ColoniesLost",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ColoniesWon",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "EmpireAge",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Fraction",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlanetsPlundered",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlayingSince",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PowerRating",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalColonies",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPlanets",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Artifacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArtifactId = table.Column<int>(type: "int", nullable: false),
                    ArtifactName = table.Column<int>(type: "int", nullable: false),
                    ArtifactType = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false),
                    MaxTotal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artifacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artifacts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CyrilClassResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SmallCyrilHull = table.Column<bool>(type: "bit", nullable: false),
                    SmallCyrilHullTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    MediumCyrilHull = table.Column<bool>(type: "bit", nullable: false),
                    MediumCyrilHullTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    LargeCyrilHull = table.Column<bool>(type: "bit", nullable: false),
                    LargeCyrilHullTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    CyrilCorvette = table.Column<bool>(type: "bit", nullable: false),
                    CyrilCorvetteTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    CyrilFrigate = table.Column<bool>(type: "bit", nullable: false),
                    CyrilFrigateTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    CyrilDestroyer = table.Column<bool>(type: "bit", nullable: false),
                    CyrilDestroyerTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    CyrilCruiser = table.Column<bool>(type: "bit", nullable: false),
                    CyrilCruiserTurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyrilClassResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CyrilClassResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EClassResearches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExaHertzClassRailgun = table.Column<bool>(type: "bit", nullable: false),
                    ExaHertzClassRailgunTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    ZettaHertzClassRailgun = table.Column<bool>(type: "bit", nullable: false),
                    ZettaHertzClassRailgunTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    EClassFighter = table.Column<bool>(type: "bit", nullable: false),
                    EClassFighterTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    EClassFrigate = table.Column<bool>(type: "bit", nullable: false),
                    EClassFrigateTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    EClassDestroyer = table.Column<bool>(type: "bit", nullable: false),
                    EClassDestroyerTurnsRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EClassResearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EClassResearches_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Infrastructers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Housing = table.Column<int>(type: "int", nullable: false),
                    Commercial = table.Column<int>(type: "int", nullable: false),
                    Agriculture = table.Column<int>(type: "int", nullable: false),
                    Industry = table.Column<int>(type: "int", nullable: false),
                    Mining = table.Column<int>(type: "int", nullable: false),
                    TurnsRequired = table.Column<int>(type: "int", nullable: false),
                    TurnsRemaining = table.Column<int>(type: "int", nullable: false),
                    TotalLevels = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infrastructers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Infrastructers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentTurns = table.Column<int>(type: "int", nullable: false),
                    MaxTurns = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Turns_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_ApplicationUserId",
                table: "Artifacts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CyrilClassResearches_ApplicationUserId",
                table: "CyrilClassResearches",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EClassResearches_ApplicationUserId",
                table: "EClassResearches",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Infrastructers_ApplicationUserId",
                table: "Infrastructers",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turns_ApplicationUserId",
                table: "Turns",
                column: "ApplicationUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artifacts");

            migrationBuilder.DropTable(
                name: "CyrilClassResearches");

            migrationBuilder.DropTable(
                name: "EClassResearches");

            migrationBuilder.DropTable(
                name: "Infrastructers");

            migrationBuilder.DropTable(
                name: "Turns");

            migrationBuilder.DropColumn(
                name: "BattlesLost",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BattlesWon",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ColoniesExplored",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ColoniesLost",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ColoniesWon",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmpireAge",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Fraction",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PlanetsPlundered",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PlayingSince",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PowerRating",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalColonies",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalPlanets",
                table: "AspNetUsers");
        }
    }
}
