using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class npcs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FederationMessages_AspNetUsers_SenderId",
                table: "FederationMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_FederationMessages_Federations_FederationsId",
                table: "FederationMessages");

            migrationBuilder.DropIndex(
                name: "IX_FederationMessages_FederationsId",
                table: "FederationMessages");

            migrationBuilder.DropColumn(
                name: "FederationsId",
                table: "FederationMessages");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeAcquired",
                table: "Planets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Federations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FederationDescription",
                table: "Federations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalPlanets",
                table: "Federations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPowerating",
                table: "Federations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FederationId",
                table: "FederationMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FederationApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FederationId = table.Column<int>(type: "int", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FederationApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FederationApplications_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FederationApplications_Federations_FederationId",
                        column: x => x.FederationId,
                        principalTable: "Federations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
    name: "FederationWars",
    columns: table => new
    {
        Id = table.Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        AttackerFederationId = table.Column<int>(type: "int", nullable: false),
        DefenderFederationId = table.Column<int>(type: "int", nullable: false),
        DeclaredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_FederationWars", x => x.Id);
        table.ForeignKey(
            name: "FK_FederationWars_Federations_AttackerFederationId",
            column: x => x.AttackerFederationId,
            principalTable: "Federations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade); // keep cascade here

        table.ForeignKey(
            name: "FK_FederationWars_Federations_DefenderFederationId",
            column: x => x.DefenderFederationId,
            principalTable: "Federations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict); // change to Restrict or NoAction
    });

            migrationBuilder.CreateTable(
                name: "MarketPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarketType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketPosts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NPCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FederationId = table.Column<int>(type: "int", nullable: true),
                    PowerRating = table.Column<int>(type: "int", nullable: false),
                    PlayingSince = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmpireAge = table.Column<double>(type: "float", nullable: false),
                    BattlesWon = table.Column<int>(type: "int", nullable: false),
                    BattlesLost = table.Column<int>(type: "int", nullable: false),
                    ColoniesWon = table.Column<int>(type: "int", nullable: false),
                    ColoniesLost = table.Column<int>(type: "int", nullable: false),
                    ColoniesExplored = table.Column<int>(type: "int", nullable: false),
                    PlanetsPlundered = table.Column<int>(type: "int", nullable: false),
                    Faction = table.Column<int>(type: "int", nullable: false),
                    TotalColonies = table.Column<int>(type: "int", nullable: false),
                    TotalPlanets = table.Column<int>(type: "int", nullable: false),
                    DamageProtection = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastAction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArtifactShield = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NPCs_Federations_FederationId",
                        column: x => x.FederationId,
                        principalTable: "Federations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FederationMessages_FederationId",
                table: "FederationMessages",
                column: "FederationId");

            migrationBuilder.CreateIndex(
                name: "IX_FederationApplications_ApplicationUserId",
                table: "FederationApplications",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FederationApplications_FederationId",
                table: "FederationApplications",
                column: "FederationId");

            migrationBuilder.CreateIndex(
                name: "IX_FederationWars_AttackerFederationId",
                table: "FederationWars",
                column: "AttackerFederationId");

            migrationBuilder.CreateIndex(
                name: "IX_FederationWars_DefenderFederationId",
                table: "FederationWars",
                column: "DefenderFederationId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketPosts_ApplicationUserId",
                table: "MarketPosts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCs_FederationId",
                table: "NPCs",
                column: "FederationId");

            migrationBuilder.AddForeignKey(
                name: "FK_FederationMessages_AspNetUsers_SenderId",
                table: "FederationMessages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FederationMessages_Federations_FederationId",
                table: "FederationMessages",
                column: "FederationId",
                principalTable: "Federations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FederationMessages_AspNetUsers_SenderId",
                table: "FederationMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_FederationMessages_Federations_FederationId",
                table: "FederationMessages");

            migrationBuilder.DropTable(
                name: "FederationApplications");

            migrationBuilder.DropTable(
                name: "FederationWars");

            migrationBuilder.DropTable(
                name: "MarketPosts");

            migrationBuilder.DropTable(
                name: "NPCs");

            migrationBuilder.DropIndex(
                name: "IX_FederationMessages_FederationId",
                table: "FederationMessages");

            migrationBuilder.DropColumn(
                name: "DateTimeAcquired",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Federations");

            migrationBuilder.DropColumn(
                name: "FederationDescription",
                table: "Federations");

            migrationBuilder.DropColumn(
                name: "TotalPlanets",
                table: "Federations");

            migrationBuilder.DropColumn(
                name: "TotalPowerating",
                table: "Federations");

            migrationBuilder.DropColumn(
                name: "FederationId",
                table: "FederationMessages");

            migrationBuilder.AddColumn<int>(
                name: "FederationsId",
                table: "FederationMessages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FederationMessages_FederationsId",
                table: "FederationMessages",
                column: "FederationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_FederationMessages_AspNetUsers_SenderId",
                table: "FederationMessages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FederationMessages_Federations_FederationsId",
                table: "FederationMessages",
                column: "FederationsId",
                principalTable: "Federations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
