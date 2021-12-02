using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Portfolio.WebApp.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectCreators",
                columns: table => new
                {
                    SubjectId = table.Column<string>(maxLength: 70, nullable: false),
                    userPicUrl = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCreators", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "Certifications",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 70, nullable: false),
                    ProjectCreatorID = table.Column<string>(maxLength: 70, nullable: true),
                    CertName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CertID = table.Column<string>(maxLength: 70, nullable: true),
                    IssuingBody_Name = table.Column<string>(nullable: true),
                    IssuingBody_Logo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Certifications_ProjectCreators_ProjectCreatorID",
                        column: x => x.ProjectCreatorID,
                        principalTable: "ProjectCreators",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 70, nullable: false),
                    ProjectCreatorID = table.Column<string>(maxLength: 70, nullable: true),
                    Company = table.Column<string>(maxLength: 200, nullable: true),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    LogoUrl = table.Column<string>(nullable: true),
                    SDate = table.Column<DateTime>(nullable: false),
                    CDate = table.Column<DateTime>(nullable: false),
                    City = table.Column<string>(maxLength: 200, nullable: true),
                    MyState = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Experiences_ProjectCreators_ProjectCreatorID",
                        column: x => x.ProjectCreatorID,
                        principalTable: "ProjectCreators",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MyDegrees",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 70, nullable: false),
                    DegreeType = table.Column<string>(nullable: true),
                    ProjectCreatorID = table.Column<string>(maxLength: 70, nullable: true),
                    DegreeName = table.Column<string>(nullable: true),
                    Minors = table.Column<string>(nullable: true),
                    Institution = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    MyState = table.Column<string>(nullable: true),
                    Graduated = table.Column<bool>(nullable: false),
                    GraduationYear = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDegrees", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MyDegrees_ProjectCreators_ProjectCreatorID",
                        column: x => x.ProjectCreatorID,
                        principalTable: "ProjectCreators",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 70, nullable: false),
                    ProjectName = table.Column<string>(maxLength: 200, nullable: true),
                    ProjectCreatorID = table.Column<string>(maxLength: 70, nullable: true),
                    Description = table.Column<string>(maxLength: 700, nullable: true),
                    Started = table.Column<DateTime>(nullable: false),
                    Completed = table.Column<DateTime>(nullable: false),
                    Banner = table.Column<string>(nullable: true),
                    Published = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectCreators_ProjectCreatorID",
                        column: x => x.ProjectCreatorID,
                        principalTable: "ProjectCreators",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 70, nullable: false),
                    ExperienceID = table.Column<string>(maxLength: 70, nullable: true),
                    MyTitle = table.Column<string>(maxLength: 200, nullable: true),
                    MyRole = table.Column<string>(maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Roles_Experiences_ExperienceID",
                        column: x => x.ExperienceID,
                        principalTable: "Experiences",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 70, nullable: false),
                    Link = table.Column<string>(nullable: true),
                    ProjectID = table.Column<string>(maxLength: 70, nullable: false),
                    Service = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Links_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPublishHistory",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 70, nullable: false),
                    ProjectID = table.Column<string>(maxLength: 70, nullable: true),
                    PublishedOn = table.Column<DateTime>(nullable: false),
                    UnPublishedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPublishHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProjectPublishHistory_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectRequirements",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 70, nullable: false),
                    ProjectID = table.Column<string>(maxLength: 70, nullable: true),
                    Requirement = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRequirements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProjectRequirements_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certifications_ProjectCreatorID",
                table: "Certifications",
                column: "ProjectCreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_ProjectCreatorID",
                table: "Experiences",
                column: "ProjectCreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Links_ProjectID",
                table: "Links",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_MyDegrees_ProjectCreatorID",
                table: "MyDegrees",
                column: "ProjectCreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPublishHistory_ProjectID",
                table: "ProjectPublishHistory",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRequirements_ProjectID",
                table: "ProjectRequirements",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectCreatorID",
                table: "Projects",
                column: "ProjectCreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ExperienceID",
                table: "Roles",
                column: "ExperienceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certifications");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "MyDegrees");

            migrationBuilder.DropTable(
                name: "ProjectPublishHistory");

            migrationBuilder.DropTable(
                name: "ProjectRequirements");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Experiences");

            migrationBuilder.DropTable(
                name: "ProjectCreators");
        }
    }
}
