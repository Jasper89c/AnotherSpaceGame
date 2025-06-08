using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AnotherSpaceGame.Data.Migrations
{
    /// <inheritdoc />
    public partial class npcs2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FederationWars_Federations_DefenderFederationId",
                table: "FederationWars");

            migrationBuilder.AddColumn<bool>(
                name: "IsNPC",
                table: "NPCs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNPC",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "NPCs",
                columns: new[] { "Id", "ArtifactShield", "BattlesLost", "BattlesWon", "ColoniesExplored", "ColoniesLost", "ColoniesWon", "DamageProtection", "EmpireAge", "Faction", "FederationId", "IsNPC", "LastAction", "PlanetsPlundered", "PlayingSince", "PowerRating", "TotalColonies", "TotalPlanets", "UserName" },
                values: new object[,]
                {
                    { 1, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4338), 0.0, 1, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4338), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4332), 1341191, 0, 0, "Aerwyn" },
                    { 2, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4346), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4346), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4344), 264153, 0, 0, "Beldorn" },
                    { 3, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4350), 0.0, 2, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4350), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4349), 349341, 0, 0, "Corlith" },
                    { 4, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4354), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4354), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4352), 1452950, 0, 0, "Dranor" },
                    { 5, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4358), 0.0, 6, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4358), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4357), 359697, 0, 0, "Eldras" },
                    { 6, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4363), 0.0, 2, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4363), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4361), 480658, 0, 0, "Fenwyn" },
                    { 7, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4367), 0.0, 2, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4367), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4366), 1018623, 0, 0, "Galmir" },
                    { 8, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4370), 0.0, 2, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4370), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4369), 771224, 0, 0, "Helnar" },
                    { 9, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4402), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4402), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4373), 1042140, 0, 0, "Ithril" },
                    { 10, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4407), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4407), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4405), 1631062, 0, 0, "Jarlorn" },
                    { 11, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4410), 0.0, 1, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4411), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4409), 803978, 0, 0, "Kelthas" },
                    { 12, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4414), 0.0, 5, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4415), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4413), 193873, 0, 0, "Lormir" },
                    { 13, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4418), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4418), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4417), 1635121, 0, 0, "Mornas" },
                    { 14, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4421), 0.0, 5, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4422), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4420), 102632, 0, 0, "Noryn" },
                    { 15, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4426), 0.0, 7, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4426), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4425), 308306, 0, 0, "Orvyr" },
                    { 16, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4429), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4429), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4428), 1389790, 0, 0, "Perion" },
                    { 17, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4433), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4433), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4432), 384961, 0, 0, "Quelthil" },
                    { 18, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4437), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4437), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4436), 626776, 0, 0, "Ryndras" },
                    { 19, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4441), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4441), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4440), 1644814, 0, 0, "Sorlorn" },
                    { 20, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4444), 0.0, 6, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4444), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4443), 1078408, 0, 0, "Torwyn" },
                    { 21, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4447), 0.0, 3, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4448), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4446), 1500492, 0, 0, "Ulnar" },
                    { 22, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4451), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4451), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4450), 311001, 0, 0, "Valmir" },
                    { 23, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4454), 0.0, 6, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4454), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4453), 116252, 0, 0, "Wynras" },
                    { 24, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4458), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4458), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4457), 1258515, 0, 0, "Xanthil" },
                    { 25, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4461), 0.0, 6, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4461), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4460), 511172, 0, 0, "Yorion" },
                    { 26, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4464), 0.0, 5, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4465), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4463), 589869, 0, 0, "Zeldras" },
                    { 27, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4468), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4468), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4467), 300138, 0, 0, "Aerthas" },
                    { 28, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4471), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4471), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4470), 1215050, 0, 0, "Belwyn" },
                    { 29, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4475), 0.0, 3, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4475), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4474), 115827, 0, 0, "Corwyn" },
                    { 30, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4478), 0.0, 5, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4478), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4477), 1417565, 0, 0, "Drasor" },
                    { 31, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4481), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4481), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4480), 448614, 0, 0, "Eldwyn" },
                    { 32, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4485), 0.0, 2, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4485), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4484), 739845, 0, 0, "Fenmir" },
                    { 33, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4488), 0.0, 5, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4488), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4487), 151291, 0, 0, "Galnar" },
                    { 34, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4492), 0.0, 3, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4492), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4491), 155821, 0, 0, "Helwyn" },
                    { 35, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4496), 0.0, 6, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4496), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4494), 38886, 0, 0, "Ithras" },
                    { 36, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4519), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4519), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4518), 886771, 0, 0, "Jarmir" },
                    { 37, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4523), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4524), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4522), 1006699, 0, 0, "Kelwyn" },
                    { 38, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4527), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4527), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4526), 1660243, 0, 0, "Lornas" },
                    { 39, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4531), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4531), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4530), 21101, 0, 0, "Mornor" },
                    { 40, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4535), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4535), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4533), 1313474, 0, 0, "Noryra" },
                    { 41, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4538), 0.0, 7, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4539), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4537), 1828325, 0, 0, "Ormir" },
                    { 42, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4542), 0.0, 3, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4542), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4541), 930469, 0, 0, "Pernar" },
                    { 43, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4546), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4546), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4545), 1238963, 0, 0, "Quelmir" },
                    { 44, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4549), 0.0, 1, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4549), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4548), 754942, 0, 0, "Rynwyn" },
                    { 45, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4552), 0.0, 6, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4553), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4551), 882347, 0, 0, "Sornar" },
                    { 46, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4556), 0.0, 3, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4556), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4555), 1071745, 0, 0, "Torlith" },
                    { 47, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4559), 0.0, 7, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4560), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4558), 605284, 0, 0, "Ulwyn" },
                    { 48, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4563), 0.0, 5, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4563), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4562), 1179676, 0, 0, "Valnar" },
                    { 49, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4566), 0.0, 7, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4566), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4565), 53533, 0, 0, "Wynmir" },
                    { 50, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4570), 0.0, 2, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4570), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4569), 1915992, 0, 0, "Xanwyn" },
                    { 51, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4573), 0.0, 1, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4573), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4572), 1425739, 0, 0, "Yorthas" },
                    { 52, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4576), 0.0, 3, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4577), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4575), 219738, 0, 0, "Zelwyn" },
                    { 53, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4580), 0.0, 6, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4580), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4579), 676498, 0, 0, "Aermir" },
                    { 54, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4583), 0.0, 1, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4584), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4582), 309869, 0, 0, "Beldras" },
                    { 55, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4587), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4587), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4586), 443085, 0, 0, "Corwyn" },
                    { 56, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4590), 0.0, 7, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4590), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4589), 1032986, 0, 0, "Dranor" },
                    { 57, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4593), 0.0, 5, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4594), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4592), 1535952, 0, 0, "Eldras" },
                    { 58, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4597), 0.0, 2, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4597), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4596), 1434174, 0, 0, "Fenwyn" },
                    { 59, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4600), 0.0, 7, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4601), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4599), 1110839, 0, 0, "Galmir" },
                    { 60, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4604), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4604), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4603), 1306902, 0, 0, "Helnar" },
                    { 61, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4607), 0.0, 7, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4607), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4606), 700633, 0, 0, "Ithril" },
                    { 62, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4611), 0.0, 1, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4611), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4610), 68878, 0, 0, "Jarlorn" },
                    { 63, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4614), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4614), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4613), 541905, 0, 0, "Kelthas" },
                    { 64, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4618), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4618), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4617), 444005, 0, 0, "Lormir" },
                    { 65, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4621), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4622), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4620), 853094, 0, 0, "Mornas" },
                    { 66, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4686), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4687), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4685), 1987067, 0, 0, "Noryn" },
                    { 67, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4691), 0.0, 2, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4691), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4689), 225597, 0, 0, "Orvyr" },
                    { 68, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4694), 0.0, 6, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4695), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4693), 657820, 0, 0, "Perion" },
                    { 69, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4698), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4699), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4697), 1654268, 0, 0, "Quelthil" },
                    { 70, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4702), 0.0, 6, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4702), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4701), 98536, 0, 0, "Ryndras" },
                    { 71, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4706), 0.0, 1, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4706), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4705), 1793350, 0, 0, "Sorlorn" },
                    { 72, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4709), 0.0, 6, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4710), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4708), 1853546, 0, 0, "Torwyn" },
                    { 73, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4713), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4713), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4712), 569702, 0, 0, "Ulnar" },
                    { 74, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4716), 0.0, 5, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4716), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4715), 1274434, 0, 0, "Valmir" },
                    { 75, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4720), 0.0, 2, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4720), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4719), 396733, 0, 0, "Wynras" },
                    { 76, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4723), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4723), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4722), 378344, 0, 0, "Xanthil" },
                    { 77, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4727), 0.0, 7, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4727), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4726), 473233, 0, 0, "Yorion" },
                    { 78, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4730), 0.0, 6, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4730), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4729), 1975042, 0, 0, "Zeldras" },
                    { 79, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4733), 0.0, 7, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4734), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4732), 283411, 0, 0, "Aerthas" },
                    { 80, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4737), 0.0, 2, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4737), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4736), 385614, 0, 0, "Belwyn" },
                    { 81, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4740), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4740), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4739), 1126573, 0, 0, "Corwyn" },
                    { 82, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4744), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4744), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4743), 1429271, 0, 0, "Drasor" },
                    { 83, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4747), 0.0, 3, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4747), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4746), 1202382, 0, 0, "Eldwyn" },
                    { 84, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4751), 0.0, 3, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4751), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4750), 1763640, 0, 0, "Fenmir" },
                    { 85, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4754), 0.0, 7, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4754), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4753), 706185, 0, 0, "Galnar" },
                    { 86, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4757), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4757), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4756), 935338, 0, 0, "Helwyn" },
                    { 87, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4761), 0.0, 6, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4761), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4760), 1896272, 0, 0, "Ithras" },
                    { 88, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4764), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4764), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4763), 269719, 0, 0, "Jarmir" },
                    { 89, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4768), 0.0, 1, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4768), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4767), 20643, 0, 0, "Kelwyn" },
                    { 90, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4771), 0.0, 5, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4771), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4770), 1881496, 0, 0, "Lornas" },
                    { 91, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4774), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4775), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4773), 816949, 0, 0, "Mornor" },
                    { 92, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4844), 0.0, 0, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4844), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4843), 809058, 0, 0, "Noryra" },
                    { 93, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4848), 0.0, 7, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4849), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4847), 1822714, 0, 0, "Ormir" },
                    { 94, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4852), 0.0, 1, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4853), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4851), 1782731, 0, 0, "Pernar" },
                    { 95, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4857), 0.0, 4, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4857), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4855), 677755, 0, 0, "Quelmir" },
                    { 96, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4861), 0.0, 7, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4861), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4860), 1237623, 0, 0, "Rynwyn" },
                    { 97, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4865), 0.0, 5, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4865), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4864), 1647333, 0, 0, "Sornar" },
                    { 98, 0.0m, 0, 0, 0, 0, 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4869), 0.0, 2, null, true, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4869), 0, new DateTime(2025, 6, 4, 20, 2, 34, 641, DateTimeKind.Utc).AddTicks(4868), 571810, 0, 0, "Torlith" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FederationWars_Federations_DefenderFederationId",
                table: "FederationWars",
                column: "DefenderFederationId",
                principalTable: "Federations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FederationWars_Federations_DefenderFederationId",
                table: "FederationWars");

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "NPCs",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DropColumn(
                name: "IsNPC",
                table: "NPCs");

            migrationBuilder.DropColumn(
                name: "IsNPC",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_FederationWars_Federations_DefenderFederationId",
                table: "FederationWars",
                column: "DefenderFederationId",
                principalTable: "Federations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
