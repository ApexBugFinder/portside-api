﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Portfolio.WebApp.Services;

namespace Portfolio.WebApp.Migrations
{
    [DbContext(typeof(PortfolioContext))]
    [Migration("20211113032226_projectLinkChange")]
    partial class projectLinkChange
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.Certification", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("CertID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("CertName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("IssuingBody_Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IssuingBody_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectCreatorID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.HasKey("ID");

                    b.HasIndex("ProjectCreatorID");

                    b.ToTable("Certifications");
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.Degree", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DegreeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DegreeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Graduated")
                        .HasColumnType("bit");

                    b.Property<DateTime>("GraduationYear")
                        .HasColumnType("datetime2");

                    b.Property<string>("Institution")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Minors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectCreatorID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("State")
                        .HasColumnName("MyState")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("ProjectCreatorID");

                    b.ToTable("MyDegrees");
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.Experience", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Company")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime>("Completed")
                        .HasColumnName("CDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectCreatorID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<DateTime>("Started")
                        .HasColumnName("SDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("State")
                        .HasColumnName("MyState")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("ID");

                    b.HasIndex("ProjectCreatorID");

                    b.ToTable("Experiences");
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.Project", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("Banner")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Completed")
                        .HasColumnName("CDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(700)")
                        .HasMaxLength(700);

                    b.Property<string>("ProjectCreatorID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("ProjectName")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<bool>("Published")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Started")
                        .HasColumnName("SDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ProjectCreatorID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.ProjectCreator", b =>
                {
                    b.Property<string>("SubjectId")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userPicUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubjectId");

                    b.ToTable("ProjectCreators");
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.ProjectLink", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectID")
                        .IsRequired()
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("Service")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("ID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.ProjectRequirement", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("ProjectID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("Requirement")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("ProjectID");

                    b.ToTable("ProjectRequirements");
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.PublishedHistory", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("ProjectID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<DateTime>("PublishedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UnPublishedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ProjectID");

                    b.ToTable("ProjectPublishHistory");
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.Role", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("ExperienceID")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("MyRole")
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<string>("MyTitle")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("ID");

                    b.HasIndex("ExperienceID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.Certification", b =>
                {
                    b.HasOne("Portfolio.PorfolioDomain.Core.Entities.ProjectCreator", "ProjectCreator")
                        .WithMany("Certifications")
                        .HasForeignKey("ProjectCreatorID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.Degree", b =>
                {
                    b.HasOne("Portfolio.PorfolioDomain.Core.Entities.ProjectCreator", "ProjectCreator")
                        .WithMany("Degrees")
                        .HasForeignKey("ProjectCreatorID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.Experience", b =>
                {
                    b.HasOne("Portfolio.PorfolioDomain.Core.Entities.ProjectCreator", "ProjectCreator")
                        .WithMany("Experiences")
                        .HasForeignKey("ProjectCreatorID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.Project", b =>
                {
                    b.HasOne("Portfolio.PorfolioDomain.Core.Entities.ProjectCreator", "ProjectOwner")
                        .WithMany("Projects")
                        .HasForeignKey("ProjectCreatorID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.ProjectLink", b =>
                {
                    b.HasOne("Portfolio.PorfolioDomain.Core.Entities.Project", "Project")
                        .WithMany("ProjectLinks")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.ProjectRequirement", b =>
                {
                    b.HasOne("Portfolio.PorfolioDomain.Core.Entities.Project", "Project")
                        .WithMany("ProjectRequirements")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.PublishedHistory", b =>
                {
                    b.HasOne("Portfolio.PorfolioDomain.Core.Entities.Project", "Project")
                        .WithMany("ProjectHistory")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Portfolio.PorfolioDomain.Core.Entities.Role", b =>
                {
                    b.HasOne("Portfolio.PorfolioDomain.Core.Entities.Experience", "Experience")
                        .WithMany("Roles")
                        .HasForeignKey("ExperienceID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
