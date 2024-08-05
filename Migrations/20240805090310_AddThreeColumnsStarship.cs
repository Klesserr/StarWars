using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarWars.Migrations
{
    /// <inheritdoc />
    public partial class AddThreeColumnsStarship : Migration
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

            migrationBuilder.DropColumn(
                name: "Gravity",
                table: "Starship");

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Starship",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Armament",
                table: "Starship",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Starship",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxPassengers",
                table: "Starship",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "Climate",
                table: "Planet",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "StarshipName",
                table: "People",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "PlanetName",
                table: "People",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "People",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.DropColumn(
                name: "Armament",
                table: "Starship");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Starship");

            migrationBuilder.DropColumn(
                name: "MaxPassengers",
                table: "Starship");

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Starship",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gravity",
                table: "Starship",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Climate",
                table: "Planet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StarshipName",
                table: "People",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlanetName",
                table: "People",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
