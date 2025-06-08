using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class _009 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
        table: "Ships",
        columns: new[] { "ShipName", "ShipType", "PowerRating", "Range", "Weapon", "Hull", "EnergyWeapon", "KineticWeapon", "MissileWeapon", "ChemicalWeapon", "EnergyShield", "KineticShield", "MissileShield", "ChemicalShield", "NoDefense", "NoRetal", "CapChance", "Cost", "Upkeep", "TerranMetal", "RedCrystal", "WhiteCrystal", "Rutile", "Composite", "StrafezOrganism" },
        values: new object[,]
        {
            { "G.Agate", 0, 13135, 7, 7885, 15760, 0, 7885, 0, 0, 1.2m, 0.9m, 0.5m, 1.1m, false, false, 0m, 208814, 1032, 0, 9, 9, 0, 10, 0 },
            { "G.Amethyst", 1, 18018, 9, 15000, 15000, 15000, 0, 0, 0, 0.5m, 1.2m, 1.05m, 0.7m, true, true, 0m, 329891, 1622, 0, 40, 30, 0, 12, 0 },
            { "G.Quartz", 2, 31108, 2, 12132, 68437, 0, 12132, 0, 0, 0.5m, 1.3m, 1.4m, 1m, false, false, 0m, 740938, 3236, 0, 75, 75, 0, 37, 0 },
            { "G.Corundum", 3, 55028, 6, 30000, 65000, 12500, 10000, 7500, 0, 0.7m, 0.5m, 1.3m, 1m, true, true, 0m, 855858, 5906, 0, 80, 80, 10, 40, 0 },
            { "G.Fluorite", 4, 69768, 1, 30000, 160000, 30000, 0, 0, 0, 1.2m, 0.7m, 0.7m, 1.2m, true, true, 0m, 1173120, 9991, 0, 80, 80, 9, 65, 0 },
            { "G.Diamond", 7, 101858, 4, 20000, 175200, 10000, 10000, 0, 0, 0.2m, 1.05m, 0.75m, 0.85m, false, false, 0m, 1412628, 24849, 0, 200, 200, 9, 105, 0 },
            { "L.Garnet", 2, 16303, 7, 8000, 20000, 0, 0, 8000, 0, 1m, 1.15m, 0.75m, 0.9m, true, true, 0m, 248980, 2401, 9, 30, 30, 50, 0, 0 },
            { "L.Emerald", 3, 41636, 8, 30000, 12000, 0, 0, 30000, 0, 1.4m, 0.8m, 0.7m, 1.6m, false, true, 0m, 647672, 7683, 30, 80, 80, 100, 0, 0 },
            { "L.Topaz", 4, 63805, 5, 25000, 110000, 0, 0, 25000, 0, 0.65m, 1.2m, 0.15m, 1.45m, true, true, 0m, 980322, 8586, 45, 39, 39, 137, 0, 0 },
            { "Tourmaline", 0, 6510, 1, 9800, 5000, 0, 6000, 3800, 0, 1.1m, 0.55m, 1.2m, 0.45m, true, true, 0m, 132476, 245, 0, 6, 6, 8, 8, 0 },
            { "Ruby", 1, 11268, 3, 13000, 16000, 0, 13000, 0, 0, 1.65m, 0.75m, 1.55m, 0.3m, true, true, 0m, 237431, 748, 0, 12, 12, 12, 12, 0 },
            { "K.Hun-Li", 3, 87273, 4, 36000, 95000, 0, 12000, 0, 24000, 0.2m, 0.2m, 1m, 0.6m, false, false, 0m, 1093100, 15477, 130, 65, 65, 12, 12, 13 },
            { "K.Hun-Zen", 4, 80316, 3, 28000, 76000, 11000, 0, 0, 17000, 0.01m, 0.01m, 0.75m, 0.75m, true, false, 0m, 1080512, 7978, 130, 50, 50, 75, 50, 75 },
            { "K.Hun-Xe", 4, 8749, 8, 7000, 8000, 0, 0, 3500, 3500, 1.65m, 1.4m, 0.65m, 1.1m, false, true, 0m, 139754, 933, 13, 28, 18, 9, 6, 3 },
            { "S.Lapiz", 9, 25, 1, 0, 5, 0, 0, 0, 0, 0.5m, 0.5m, 0.8m, 0.8m, true, false, 0m, 1074, 50, 0, 0, 0, 0, 0, 0 },
            { "S.Opal", 9, 75, 1, 0, 5, 0, 0, 0, 0, 0.5m, 0.5m, 0.8m, 0.8m, true, false, 0m, 3167, 150, 0, 0, 0, 0, 0, 0 }
        }
    );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
