using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class shipsCA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanCapture",
                table: "Ships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ImmuneToCapture",
                table: "Ships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UnusedLevels",
                table: "Infrastructers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CounterAttacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TargetUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounterAttacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CounterAttacks_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Missions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Mission1 = table.Column<bool>(type: "bit", nullable: false),
                    Mission2 = table.Column<bool>(type: "bit", nullable: false),
                    Mission3 = table.Column<bool>(type: "bit", nullable: false),
                    Mission4 = table.Column<bool>(type: "bit", nullable: false),
                    Mission5 = table.Column<bool>(type: "bit", nullable: false),
                    Mission6 = table.Column<bool>(type: "bit", nullable: false),
                    Mission7 = table.Column<bool>(type: "bit", nullable: false),
                    Mission8 = table.Column<bool>(type: "bit", nullable: false),
                    Mission9 = table.Column<bool>(type: "bit", nullable: false),
                    Mission10 = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Missions_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CapsuleLab = table.Column<bool>(type: "bit", nullable: false),
                    CapsuleLabTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    CapsuleLabCreditsRequired = table.Column<int>(type: "int", nullable: false),
                    CapsuleLabUnlockTimer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KalZulLoktar = table.Column<bool>(type: "bit", nullable: false),
                    KalZulLoktarTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    KalZulLoktarCreditsRequired = table.Column<int>(type: "int", nullable: false),
                    KalZulLoktarUnlockTimer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KalZulHektar = table.Column<bool>(type: "bit", nullable: false),
                    KalZulHektarTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    KalZulHektarCreditsRequired = table.Column<int>(type: "int", nullable: false),
                    KalZulHektarUnlockTimer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Itech = table.Column<bool>(type: "bit", nullable: false),
                    ItechTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    ItechCreditsRequired = table.Column<int>(type: "int", nullable: false),
                    ItechUnlockTimer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnreverseEngineering = table.Column<bool>(type: "bit", nullable: false),
                    UnreverseEngineeringTurnsRequired = table.Column<int>(type: "int", nullable: false),
                    UnreverseEngineeringCreditsRequired = table.Column<int>(type: "int", nullable: false),
                    UnreverseEngineeringUnlockTimer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProjects_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CounterAttacks_ApplicationUserId",
                table: "CounterAttacks",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_ApplicationUserId",
                table: "Missions",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProjects_ApplicationUserId",
                table: "UserProjects",
                column: "ApplicationUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CounterAttacks");

            migrationBuilder.DropTable(
                name: "Missions");

            migrationBuilder.DropTable(
                name: "UserProjects");

            migrationBuilder.DropColumn(
                name: "CanCapture",
                table: "Ships");

            migrationBuilder.DropColumn(
                name: "ImmuneToCapture",
                table: "Ships");

            migrationBuilder.DropColumn(
                name: "UnusedLevels",
                table: "Infrastructers");
        }
    }
}
