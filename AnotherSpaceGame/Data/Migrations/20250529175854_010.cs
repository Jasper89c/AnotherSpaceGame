using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class _010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
         table: "Ships",
         columns: new[] { "ShipName", "ShipType", "PowerRating", "Range", "Weapon", "Hull", "EnergyWeapon", "KineticWeapon", "MissileWeapon", "ChemicalWeapon", "EnergyShield", "KineticShield", "MissileShield", "ChemicalShield", "NoDefense", "NoRetal", "CapChance", "Cost", "Upkeep", "TerranMetal", "RedCrystal", "WhiteCrystal", "Rutile", "Composite", "StrafezOrganism" },
         values: new object[,]
         {
            // ... (previous ships)
            { "F.Axe", 2, 178, 2, 65, 500, 0, 65, 0, 0, 1.2m, 0.8m, 1m, 1m, false, false, 0m, 1898, 21, 2, 1, 1, 1, 0, 0 },
            { "F.Broadsword", 3, 1116, 8, 781, 334, 0, 781, 0, 0, 1.3m, 0.9m, 0.9m, 0.9m, false, false, 0m, 12119, 100, 0, 4, 0, 2, 4, 0 },
            { "F.Sword", 3, 1152, 5, 600, 2000, 0, 600, 0, 0, 1.2m, 0.8m, 1m, 1m, false, false, 0m, 11400, 105, 10, 2, 2, 2, 0, 0 },
            { "F.Spear", 4, 1512, 3, 400, 4000, 0, 400, 0, 0, 1.2m, 0.8m, 1m, 1m, false, false, 0m, 14670, 261, 14, 2, 2, 2, 0, 0 },
            { "E.Aurora", 0, 33, 6, 24, 30, 4, 0, 20, 0, 0.65m, 1m, 1m, 0.8m, false, true, 0m, 392, 9, 0, 0, 0, 0, 2, 0 },
            { "E.Nova", 2, 850, 5, 400, 1500, 0, 0, 400, 0, 1.2m, 1m, 0.6m, 1.1m, true, true, 0m, 8285, 137, 0, 1, 1, 1, 3, 0 },
            { "E.Pulsar", 3, 1376, 8, 1100, 825, 0, 0, 1100, 0, 1m, 1m, 1m, 1m, false, false, 0m, 15345, 243, 0, 2, 2, 2, 5, 0 },
            { "C.Aries", 1, 51, 1, 20, 200, 20, 0, 0, 0, 0.9m, 0.9m, 1.2m, 1m, false, false, 0m, 617, 2, 0, 1, 1, 0, 1, 0 },
            { "C.Gemini", 2, 243, 2, 53, 1000, 53, 0, 0, 0, 0.9m, 0.9m, 1.2m, 1m, false, false, 0m, 2784, 22, 0, 3, 3, 0, 3, 0 },
            { "C.Taurus", 3, 1488, 4, 400, 4000, 200, 200, 0, 0, 0.9m, 0.9m, 1.2m, 1m, false, false, 0m, 14595, 295, 0, 4, 4, 0, 4, 0 },
            { "C.Cancer", 4, 1804, 3, 350, 6000, 350, 0, 0, 0, 0.85m, 0.85m, 1.3m, 1m, false, false, 0m, 18355, 407, 0, 5, 5, 0, 5, 0 },
            { "C.Leo", 4, 3006, 5, 1300, 4500, 650, 650, 0, 0, 0.9m, 0.95m, 1.25m, 1m, true, true, 0m, 27401, 403, 0, 5, 7, 0, 6, 0 },
            { "Small Strafez Fodder", 10, 55, 1, 0, 200, 0, 0, 0, 0, 1.1m, 1.75m, 0.5m, 0.5m, true, false, 0m, 535, 2, 0, 0, 0, 0, 0, 2 },
            { "Large Strafez Fodder", 10, 205, 1, 0, 800, 0, 0, 0, 0, 1.1m, 1.75m, 0.5m, 0.5m, true, false, 0m, 2035, 6, 0, 0, 0, 0, 0, 10 },
            { "Small Strafez Runner", 10, 115, 1, 115, 1, 0, 0, 0, 115, 1m, 1.95m, 0.01m, 0.75m, false, true, 0m, 1046, 0, 0, 0, 0, 0, 0, 2 },
            { "Large Strafez Runner", 10, 435, 5, 400, 2, 0, 0, 0, 400, 1m, 1.95m, 0.75m, 0.01m, true, true, 0m, 4247, 6, 0, 0, 0, 0, 0, 10 },
            { "Strafez Queen", 10, 896, 7, 700, 25, 0, 0, 0, 700, 1m, 1.95m, 0.01m, 0.01m, true, true, 0m, 8121, 14, 0, 0, 0, 0, 0, 20 },
            { "Strafez King", 10, 890, 8, 550, 200, 0, 0, 0, 550, 1.15m, 1.15m, 0.1m, 0.1m, true, true, 0m, 7580, 20, 0, 0, 0, 0, 0, 40 },
            { "Light 0-drone", 0, 5, 1, 5, 5, 5, 0, 0, 0, 1.2m, 1m, 0.85m, 1.2m, false, false, 0m, 66, 0, 0, 0, 0, 0, 1, 0 },
            { "UW.Endsieg", 3, 2032, 7, 1600, 2000, 500, 300, 700, 100, 0.7m, 0.4m, 0.9m, 0.5m, false, false, 0m, 20820, 0, 0, 0, 0, 3, 3, 3 },
            { "UW.Erster Sieg", 4, 4441, 6, 3500, 3500, 0, 1250, 2250, 0, 1.45m, 0.65m, 0.65m, 1m, false, true, 0m, 44264, 0, 2, 2, 5, 0, 0, 0 },
            { "UW.Vergessener Sieg", 5, 11000, 7, 8500, 12750, 0, 0, 0, 8500, 1m, 0.1m, 1.55m, 1m, false, true, 0m, 113607, 0, 45, 45, 45, 45, 45, 45 },
            { "UW.Zweiter Sieg", 6, 27706, 5, 18500, 27000, 7000, 4000, 7500, 0, 0.85m, 1.05m, 0.35m, 0.35m, true, true, 0m, 265950, 0, 100, 100, 100, 100, 100, 100 }
         }
     );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
