using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarWars.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsPeople : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlanetName",
                table: "People",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StarshipName",
                table: "People",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_People_PlanetName",
                table: "People",
                column: "PlanetName");

            migrationBuilder.CreateIndex(
                name: "IX_People_StarshipName",
                table: "People",
                column: "StarshipName");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Planet_PlanetName",
                table: "People",
                column: "PlanetName",
                principalTable: "Planet",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_People_Starship_StarshipName",
                table: "People",
                column: "StarshipName",
                principalTable: "Starship",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Planet_PlanetName",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_People_Starship_StarshipName",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_PlanetName",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_StarshipName",
                table: "People");

            migrationBuilder.DropColumn(
                name: "PlanetName",
                table: "People");

            migrationBuilder.DropColumn(
                name: "StarshipName",
                table: "People");
        }
    }
}
