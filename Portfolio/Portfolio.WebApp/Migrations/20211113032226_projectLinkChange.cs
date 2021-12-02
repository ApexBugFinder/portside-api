using Microsoft.EntityFrameworkCore.Migrations;

namespace Portfolio.WebApp.Migrations
{
    public partial class projectLinkChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Links",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Links",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Links");
        }
    }
}
