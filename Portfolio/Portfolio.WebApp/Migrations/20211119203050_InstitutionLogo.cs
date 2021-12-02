using Microsoft.EntityFrameworkCore.Migrations;

namespace Portfolio.WebApp.Migrations
{
    public partial class InstitutionLogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstitutionLogo",
                table: "MyDegrees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstitutionLogo",
                table: "MyDegrees");
        }
    }
}
