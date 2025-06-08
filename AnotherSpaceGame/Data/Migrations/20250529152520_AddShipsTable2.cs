using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddShipsTable2 : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Ships",
                columns: new[] { "ShipName", "ShipType", "PowerRating", "Range", "Weapon", "Hull", "EnergyWeapon", "KineticWeapon", "MissileWeapon", "ChemicalWeapon", "EnergyShield", "KineticShield", "MissileShield", "ChemicalShield", "NoDefense", "NoRetal", "CapChance", "Cost", "Upkeep", "TerranMetal", "RedCrystal", "WhiteCrystal", "Rutile", "Composite", "StrafezOrganism" },
                values: new object[,]
                {
            { "T.Ryu-jin", 0, 8, 1, 5, 10, 4, 0, 0, 1, 1.3m, 1m, 0.1m, 1.2m, false, false, 0m, 88, 4, 0, 0, 0, 0, 0, 0 },
            { "T.Maru", 1, 51, 2, 35, 100, 35, 0, 0, 0, 1m, 0.5m, 1.15m, 1.15m, false, false, 0m, 590, 39, 1, 0, 0, 0, 0, 0 },
            { "T.Sentouki", 2, 174, 5, 56, 500, 28, 28, 0, 0, 1.05m, 0.6m, 1.25m, 1m, false, false, 0m, 1855, 248, 2, 0, 0, 0, 0, 0 },
            { "T.Garuda", 3, 956, 8, 600, 925, 600, 0, 0, 0, 1.1m, 0.7m, 1.1m, 1.1m, true, true, 0m, 9076, 489, 10, 0, 0, 0, 0, 0 },
            { "T.Fenrir", 4, 1512, 7, 950, 2000, 450, 500, 0, 0, 1.15m, 0.9m, 0.75m, 1m, true, true, 0m, 14450, 900, 6, 1, 1, 0, 0, 0 },
            { "T.Kalieum", 4, 1701, 6, 450, 4000, 350, 0, 0, 100, 1.4m, 1.05m, 0.5m, 0.2m, false, false, 0m, 15435, 1655, 10, 0, 0, 0, 0, 0 },
            { "T.Empereur", 5, 5881, 6, 2600, 8000, 2600, 0, 0, 0, 0.4m, 0.9m, 1.2m, 1.35m, true, true, 0m, 52475, 5844, 80, 0, 0, 0, 0, 0 },
            { "Tyr", 6, 13035, 5, 10000, 12000, 500, 500, 9000, 0, 0.9m, 1.2m, 0.6m, 0.8m, false, true, 0m, 130245, 22224, 180, 220, 220, 110, 120, 0 },
            { "Nirvana", 6, 18030, 4, 12000, 20000, 5000, 5000, 2000, 0, 0.5m, 0.75m, 0.75m, 1.6m, false, true, 0m, 175410, 23055, 450, 200, 200, 200, 200, 0 },
            { "Chimaera", 7, 21393, 3, 10000, 45000, 4000, 6000, 0, 0, 1.3m, 0.5m, 0.7m, 1m, false, false, 0m, 213765, 25452, 225, 100, 100, 100, 100, 0 },
            { "Scorpion", 7, 43681, 5, 12000, 35000, 2000, 2000, 8000, 0, 0.7m, 0.8m, 0.45m, 0.75m, false, false, 0m, 237775, 61936, 300, 85, 85, 85, 100, 0 },
            { "P.Apollo", 1, 66, 2, 80, 45, 0, 0, 80, 0, 1m, 1.2m, 0.65m, 0.7m, true, true, 0m, 811, 19, 0, 0, 1, 1, 0, 0 },
            { "P.Odin", 2, 201, 8, 212, 125, 0, 0, 212, 0, 1m, 1.2m, 0.65m, 1m, true, true, 0m, 2311, 118, 0, 0, 2, 2, 0, 0 },
            { "P.Thor", 3, 1624, 9, 1600, 812, 0, 1600, 0, 0, 1m, 0.85m, 1.15m, 1m, true, true, 0m, 16905, 1067, 0, 0, 10, 10, 0, 0 },
            { "P.Zeus", 4, 1755, 10, 1400, 1000, 0, 0, 1400, 0, 0.85m, 1.1m, 0.6m, 1.1m, true, true, 0m, 16487, 1258, 0, 0, 10, 10, 0, 0 },
            { "P.Hercules", 8, 41769, 3, 21000, 50000, 0, 14000, 7000, 0, 1m, 0.25m, 0.35m, 1.3m, true, true, 0m, 378236, 59815, 368, 230, 230, 276, 552, 0 },
            { "Viator", 9, 1, 1, 0, 3, 0, 0, 0, 0, 1m, 1m, 0.05m, 1m, true, false, 0m, 27, 30, 0, 0, 0, 0, 0, 0 },
            { "Viator II", 9, 25, 1, 0, 1, 0, 0, 0, 0, 1m, 1m, 0.05m, 1m, true, false, 0m, 636, 400, 0, 0, 0, 0, 0, 0 }
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
