using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class _007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
        table: "Ships",
        columns: new[] { "ShipName", "ShipType", "PowerRating", "Range", "Weapon", "Hull", "EnergyWeapon", "KineticWeapon", "MissileWeapon", "ChemicalWeapon", "EnergyShield", "KineticShield", "MissileShield", "ChemicalShield", "NoDefense", "NoRetal", "CapChance", "Cost", "Upkeep", "TerranMetal", "RedCrystal", "WhiteCrystal", "Rutile", "Composite", "StrafezOrganism" },
        values: new object[,]
        {
            { "R.Pinnace", 1, 162, 2, 200, 100, 0, 200, 0, 0, 1.1m, 0.5m, 0.6m, 1m, false, true, 0m, 2100, 32, 0, 0, 0, 1, 1, 0 },
            { "R.Sloop", 2, 558, 6, 550, 750, 550, 0, 0, 0, 1.15m, 0.55m, 0.6m, 1.1m, false, false, 0m, 6547, 181, 0, 0, 0, 1, 2, 0 },
            { "R.Schooner", 3, 2136, 8, 1150, 2200, 900, 0, 250, 0, 1.05m, 0.2m, 1.3m, 1m, false, true, 0.5m, 19815, 856, 2, 0, 0, 3, 6, 0 },
            { "R.Snow", 4, 6804, 5, 3500, 8000, 1500, 0, 2000, 0, 1.2m, 0.5m, 0.75m, 1.2m, true, true, 0.5m, 61920, 3077, 5, 0, 0, 5, 8, 0 },
            { "R.Monitor", 6, 29751, 4, 19000, 33500, 0, 5000, 14000, 0, 0.5m, 0.75m, 1.2m, 1.3m, false, true, 0.5m, 286890, 16025, 50, 0, 0, 50, 90, 0 },
            { "R.Sovereign", 8, 74069, 3, 32000, 103696, 16000, 7000, 9000, 0, 1m, 0.35m, 1.3m, 0.5m, true, true, 0m, 665395, 40995, 0, 5, 5, 0, 0, 0 },
            { "H.Corsair", 0, 131, 1, 100, 165, 30, 0, 30, 40, 1.15m, 1.15m, 1m, 0.55m, false, true, 1m, 1414, 34, 0, 0, 0, 1, 1, 1 },
            { "H.Brigantine", 2, 286, 4, 180, 340, 0, 0, 60, 120, 0.9m, 1.1m, 0.6m, 1.25m, false, true, 1m, 2815, 100, 0, 1, 1, 1, 1, 1 },
            { "H.Galleon", 3, 2056, 9, 900, 4500, 0, 800, 100, 0, 0.7m, 1.3m, 1.25m, 0.9m, false, false, 1m, 20595, 556, 1, 1, 1, 3, 3, 0 },
            { "H.Barkentine", 4, 3246, 7, 1300, 5900, 0, 0, 1300, 0, 0.8m, 0.8m, 0.75m, 0.7m, true, false, 1m, 29360, 1190, 1, 1, 1, 3, 3, 0 },
            { "H.Suzerain", 6, 19840, 4, 12000, 23000, 0, 12000, 0, 0, 1.45m, 1m, 0.55m, 0.75m, false, true, 1m, 188810, 10452, 20, 20, 20, 30, 30, 0 },
            { "H.Overlord", 11, 79420, 3, 55000, 85000, 30000, 0, 0, 25000, 0.5m, 1.3m, 1.2m, 0.55m, false, true, 1m, 864600, 53293, 38, 38, 38, 115, 115, 0 },
            { "H.Man-O-War", 7, 450, 6, 250, 600, 100, 150, 0, 0, 0.9m, 0.75m, 0.5m, 1.2m, false, false, 1m, 4333, 192, 0, 1, 1, 2, 2, 0 }
        }
    );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
