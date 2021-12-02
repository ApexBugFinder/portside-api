using Microsoft.EntityFrameworkCore.Migrations;

namespace Portfolio.WebApp.Migrations
{
    public partial class projectDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Started",
                table: "Projects",
                newName: "SDate");

            migrationBuilder.RenameColumn(
                name: "Completed",
                table: "Projects",
                newName: "CDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SDate",
                table: "Projects",
                newName: "Started");

            migrationBuilder.RenameColumn(
                name: "CDate",
                table: "Projects",
                newName: "Completed");
        }
    }
}
