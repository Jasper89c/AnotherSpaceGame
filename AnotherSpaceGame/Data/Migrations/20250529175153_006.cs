using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class _006 : Migration
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
            { "M.Hal", 0, 23, 2, 15, 30, 15, 0, 0, 0, 1.2m, 0.8m, 1.2m, 0.6m, false, true, 0m, 293, 19, 0, 0, 0, 0, 0, 0 },
            { "M.Alium", 1, 46, 1, 26, 69, 13, 0, 13, 0, 1m, 0.7m, 1.15m, 1m, false, true, 0m, 487, 36, 0, 0, 0, 2, 0, 0 },
            { "M.Illite", 2, 182, 7, 80, 400, 0, 80, 0, 0, 0.85m, 0.8m, 0.7m, 1.05m, false, false, 0m, 1880, 213, 0, 2, 0, 0, 0, 0 },
            { "M.Epidote", 3, 936, 8, 500, 1500, 500, 0, 0, 0, 0.8m, 0.7m, 1.15m, 1.1m, false, false, 0m, 9195, 746, 0, 0, 0, 10, 0, 0 },
            { "M.Chlorite", 3, 1236, 9, 1000, 800, 0, 0, 1000, 0, 1.15m, 1m, 0.85m, 1m, false, true, 0m, 12330, 1442, 0, 10, 0, 10, 0, 0 },
            { "M.Chlor", 4, 1656, 6, 500, 5000, 500, 0, 0, 0, 1m, 0.7m, 0.9m, 0.9m, false, false, 0m, 17160, 1869, 0, 0, 0, 12, 0, 0 },
            { "M.Calcite", 4, 1701, 7, 850, 2000, 0, 0, 850, 0, 1m, 1.15m, 0.6m, 1m, false, true, 0m, 15435, 2759, 0, 15, 0, 15, 0, 0 },
            { "M.Flysch", 5, 7231, 5, 3000, 10000, 1500, 500, 1000, 0, 1m, 0.7m, 1.15m, 1.15m, false, true, 0m, 64225, 13828, 0, 0, 0, 80, 0, 0 },
            { "M.Lyth", 6, 37550, 5, 18750, 45000, 3750, 0, 15000, 0, 0.55m, 1.25m, 0.55m, 1.25m, false, true, 0m, 339386, 87676, 375, 375, 375, 1187, 187, 0 },
            { "M.Lakko", 7, 24444, 1, 23700, 32000, 7700, 8000, 8000, 0, 1.6m, 0.9m, 0.8m, 1.95m, false, true, 0m, 286791, 19790, 19, 20, 12, 112, 28, 0 },
            { "A.Aragonite", 6, 5035, 6, 2500, 6100, 1000, 1500, 0, 0, 1.15m, 1.15m, 0.75m, 0.65m, false, true, 0m, 45445, 10121, 50, 10, 10, 120, 20, 0 },
            { "A.Hoko", 7, 9945, 9, 4000, 10000, 2000, 2000, 0, 0, 1.3m, 0.6m, 0.7m, 0.85m, false, false, 0m, 82725, 8906, 6, 6, 6, 60, 10, 0 },
            { "A.Kryo", 7, 87781, 4, 22500, 150000, 7500, 15000, 0, 0, 0.9m, 0.8m, 0.5m, 1.35m, true, true, 0m, 746475, 216558, 300, 300, 270, 800, 425, 0 },
            { "G.Livid (r)", 0, 12513, 2, 7500, 20021, 0, 7500, 0, 0, 0.85m, 1.1m, 1.05m, 0.2m, true, true, 0m, 121615, 12965, 0, 100, 100, 600, 0, 0 },
            { "G.Rhyolite", 3, 1596, 2, 1756, 1000, 0, 0, 0, 1756, 0.5m, 0.5m, 1.35m, 1.2m, true, true, 0m, 15420, 500, 0, 10, 5, 10, 0, 20 },
            { "G.Sapphire", 3, 17624, 4, 14000, 18000, 3000, 11000, 0, 0, 1.45m, 0.6m, 0.4m, 1.35m, true, true, 0m, 181380, 17152, 130, 130, 200, 400, 0, 0 },
            { "G.Lictor", 8, 82527, 3, 34000, 115500, 12000, 22000, 0, 0, 0.25m, 0.35m, 1m, 1.3m, true, true, 0m, 700085, 165420, 523, 445, 365, 1377, 365, 0 },
            { "Seeker", 9, 2, 1, 0, 1, 0, 0, 0, 0, 1m, 1m, 0.05m, 1.95m, true, false, 0m, 61, 60, 0, 0, 0, 0, 0, 0 },
            { "Ranger", 9, 50, 1, 0, 1, 0, 0, 0, 0, 1m, 1m, 0.05m, 1.95m, true, false, 0m, 1261, 800, 0, 0, 0, 0, 0, 0 },
            { "G.Livid", 0, 13013, 5, 12000, 25000, 0, 8000, 0, 4000, 0.1m, 1.2m, 1.15m, 0.15m, false, false, 0m, 160365, 7525, 0, 0, 0, 0, 0, 0 }
       }
   );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
