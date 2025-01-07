using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarWars.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameColumnStarship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Armament",
                table: "Starship");

            migrationBuilder.AddColumn<string>(
                name: "Class_Starship",
                table: "Starship",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class_Starship",
                table: "Starship");

            migrationBuilder.AddColumn<string>(
                name: "Armament",
                table: "Starship",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
