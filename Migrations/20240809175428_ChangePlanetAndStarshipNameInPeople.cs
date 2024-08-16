using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarWars.Migrations
{
    /// <inheritdoc />
    public partial class ChangePlanetAndStarshipNameInPeople : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Planet_PlanetName",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_People_Starship_StarshipName",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_Planet_People_PeopleId",
                table: "Planet");

            migrationBuilder.DropForeignKey(
                name: "FK_Starship_People_PeopleId",
                table: "Starship");

            migrationBuilder.DropIndex(
                name: "IX_Starship_PeopleId",
                table: "Starship");

            migrationBuilder.DropIndex(
                name: "IX_Planet_PeopleId",
                table: "Planet");

            migrationBuilder.DropIndex(
                name: "IX_People_PlanetName",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_StarshipName",
                table: "People");

            migrationBuilder.DropColumn(
                name: "PeopleId",
                table: "Starship");

            migrationBuilder.DropColumn(
                name: "PeopleId",
                table: "Planet");

            migrationBuilder.AlterColumn<string>(
                name: "StarshipName",
                table: "People",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlanetName",
                table: "People",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Order",
                table: "People",
                type: "nvarchar(24)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PeopleId",
                table: "Starship",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PeopleId",
                table: "Planet",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StarshipName",
                table: "People",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlanetName",
                table: "People",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "People",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(24)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Starship_PeopleId",
                table: "Starship",
                column: "PeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_Planet_PeopleId",
                table: "Planet",
                column: "PeopleId");

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
                principalColumn: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Starship_StarshipName",
                table: "People",
                column: "StarshipName",
                principalTable: "Starship",
                principalColumn: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Planet_People_PeopleId",
                table: "Planet",
                column: "PeopleId",
                principalTable: "People",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Starship_People_PeopleId",
                table: "Starship",
                column: "PeopleId",
                principalTable: "People",
                principalColumn: "Id");
        }
    }
}
