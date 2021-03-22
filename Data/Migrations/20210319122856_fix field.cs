using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Data.Migrations
{
    public partial class fixfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Interrests",
                table: "Users",
                newName: "Interests");

            migrationBuilder.RenameColumn(
                name: "Contry",
                table: "Users",
                newName: "Country");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Interests",
                table: "Users",
                newName: "Interrests");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Users",
                newName: "Contry");
        }
    }
}
