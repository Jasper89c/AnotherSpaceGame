using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

#nullable disable

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class shipupdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CostToBuild",
                table: "Ships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            int[] buildRates = new int[]
    {
        5600, 800, 300, 28, 18, 22, 10, 5, 3, 2, 2, 470, 120, 16, 16, 2, 80, 160, 1500, 675, 200, 28, 22, 16, 28, 9, 2, 3, 12, 3, 1, 4, 18, 3, 1, 60, 160, 1, 200, 55, 16, 10, 2, 1, 275, 135, 12, 15, 3, 1, 90, 60, 3, 15, 6, 180, 700, 85, 100, 18, 18, 16, 3, 6, 5, 3, 75, 8, 20, 18, 3, 2, 2, 1, 1, 1, 5, 2, 1, 6, 4, 1, 1, 4, 30, 20, 130, 25, 20, 24, 900, 30, 20, 100, 40, 20, 16, 10, 800, 180, 400, 100, 40, 50, 7000, 0, 0, 0, 0
    };

            int[] costToBuild = new int[]
            {
        88, 590, 1855, 9076, 14450, 15435, 52475, 130245, 175410, 213765, 237775, 811, 2311, 16905, 16487, 378236, 27, 636, 293, 487, 1880, 9195, 12330, 17160, 15435, 64225, 339386, 286791, 45445, 82725, 746475, 121615, 15420, 181380, 700085, 61, 1261, 160365, 2100, 6547, 19815, 61920, 286890, 665395, 1414, 2815, 20595, 29360, 188810, 864600, 4333, 4965, 170980, 19240, 134730, 511, 655, 6165, 3332, 15495, 19440, 19180, 244195, 57160, 122475, 293805, 4117, 41955, 12780, 15685, 208814, 329891, 740938, 855858, 1173120, 1412628, 248980, 647672, 980322, 132476, 237431, 1093100, 1080512, 139754, 1074, 3167, 1898, 12119, 11400, 14670, 392, 8285, 15345, 617, 2784, 14595, 18355, 27401, 535, 2035, 1046, 4247, 8121, 7580, 66, 20820, 44264, 113607, 265950
            };

            int startId = 219;
            for (int i = 0; i < buildRates.Length; i++)
            {
                int shipId = startId + i;
                migrationBuilder.Sql($"UPDATE Ships SET BuildRate = {buildRates[i]}, CostToBuild = {costToBuild[i]} WHERE Id = {shipId};");
            }


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostToBuild",
                table: "Ships");
        }
    }
}
