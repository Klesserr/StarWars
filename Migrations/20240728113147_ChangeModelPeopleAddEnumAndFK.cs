using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarWars.Migrations
{
    /// <inheritdoc />
    public partial class ChangeModelPeopleAddEnumAndFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "Race",
                table: "People",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "People",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Starship_PeopleId",
                table: "Starship",
                column: "PeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_Planet_PeopleId",
                table: "Planet",
                column: "PeopleId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "PeopleId",
                table: "Starship");

            migrationBuilder.DropColumn(
                name: "PeopleId",
                table: "Planet");

            migrationBuilder.AlterColumn<string>(
                name: "Race",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Order",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
