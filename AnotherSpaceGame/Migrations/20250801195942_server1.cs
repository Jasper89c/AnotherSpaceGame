using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnotherSpaceGame.Migrations
{
    /// <inheritdoc />
    public partial class server1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TempleHeight",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempleHeight",
                table: "AspNetUsers");
        }
    }
}
