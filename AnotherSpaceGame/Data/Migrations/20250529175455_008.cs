using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class _008 : Migration
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
            { "V.Borrelly", 3, 452, 7, 400, 360, 0, 100, 0, 300, 0.9m, 1.1m, 1.2m, 0.35m, true, true, 0.75m, 4965, 238, 2, 0, 0, 2, 2, 4 },
            { "B.Chiron", 7, 18482, 2, 8000, 30000, 0, 3000, 0, 5000, 1.8m, 1m, 0.45m, 0.8m, false, false, 0m, 170980, 14782, 230, 0, 0, 230, 230, 300 },
            { "B.Cronus", 4, 2016, 6, 1300, 2000, 450, 0, 0, 850, 1.35m, 0.75m, 0.5m, 1m, true, true, 0m, 19240, 953, 10, 0, 0, 10, 10, 20 },
            { "V.Triton", 4, 8982, 5, 4250, 11200, 0, 1500, 2750, 0, 0.8m, 1.05m, 0.65m, 1.2m, false, true, 0.75m, 134730, 8820, 40, 40, 40, 40, 0, 0 },
            { "Kohoutek", 9, 20, 1, 0, 1, 0, 0, 0, 0, 0.6m, 0.8m, 1m, 1m, true, false, 0m, 511, 200, 0, 0, 0, 0, 0, 0 },
            { "Eel", 0, 60, 2, 50, 50, 0, 25, 25, 0, 1.2m, 0.8m, 0.9m, 1.05m, false, true, 0m, 655, 8, 0, 0, 0, 0, 0, 0 },
            { "Manta", 1, 512, 2, 384, 1024, 192, 0, 192, 0, 1.65m, 1.35m, 0.35m, 0.65m, true, false, 0m, 6165, 63, 0, 0, 0, 0, 0, 0 },
            { "Piranha", 2, 379, 6, 120, 750, 60, 60, 0, 0, 0.95m, 1.2m, 0.85m, 1.05m, false, false, 0m, 3332, 54, 0, 0, 0, 0, 0, 0 },
            { "Tiger", 3, 1496, 9, 1200, 1500, 0, 0, 1200, 0, 0.7m, 1.2m, 0.8m, 1m, true, true, 0m, 15495, 254, 0, 0, 0, 0, 0, 0 },
            { "Barracuda", 3, 2272, 7, 800, 3000, 600, 200, 0, 0, 1.15m, 0.6m, 1.4m, 0.2m, false, true, 0m, 19440, 653, 0, 0, 0, 0, 0, 0 },
            { "Shark", 4, 2052, 4, 500, 6250, 250, 250, 0, 0, 1.2m, 1m, 0.6m, 1.3m, false, false, 0m, 19180, 398, 0, 0, 0, 0, 0, 0 },
            { "Pyth", 5, 22527, 4, 20000, 20000, 7500, 7500, 5000, 0, 0.5m, 0.75m, 1.2m, 1.15m, false, false, 0m, 244195, 5547, 0, 0, 0, 0, 0, 0 },
            { "Bullhead", 4, 6336, 8, 3250, 7000, 1250, 0, 2000, 0, 0.75m, 1.35m, 0.5m, 1.25m, true, true, 0m, 57160, 1128, 0, 10, 10, 10, 10, 0 },
            { "Thresher", 5, 12181, 5, 10000, 9000, 7500, 1250, 1250, 0, 1.7m, 0.45m, 0.7m, 0.65m, true, true, 0m, 122475, 2854, 0, 11, 13, 13, 11, 0 },
            { "Angel", 5, 27847, 5, 26400, 18000, 0, 8400, 18000, 0, 0.85m, 1.3m, 0.3m, 1.1m, true, true, 0m, 293805, 8653, 0, 18, 18, 18, 18, 0 },
            { "D.Ray", 0, 317, 6, 450, 166, 110, 10, 330, 0, 1.1m, 1.25m, 0.8m, 1.25m, true, true, 0m, 4117, 42, 0, 0, 0, 0, 0, 0 },
            { "D.Hammerhead", 2, 3612, 7, 4000, 2700, 0, 1500, 2500, 0, 1.25m, 0.85m, 0.45m, 1.15m, false, true, 0m, 41955, 927, 0, 5, 5, 5, 5, 0 },
            { "D.Pike", 3, 1276, 9, 1000, 1000, 500, 500, 0, 0, 0.9m, 1.1m, 0.9m, 1.1m, false, true, 0m, 12780, 289, 0, 0, 0, 0, 0, 0 },
            { "D.Sting", 4, 1611, 8, 1250, 2200, 0, 475, 475, 300, 1.35m, 1m, 0.65m, 1m, true, false, 0m, 15685, 205, 0, 0, 0, 0, 0, 0 }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
