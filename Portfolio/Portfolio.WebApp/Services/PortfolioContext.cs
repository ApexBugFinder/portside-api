using Microsoft.EntityFrameworkCore;
using Portfolio.PorfolioDomain.Core.Entities;
using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Portfolio.WebApp.Services
{
    public class PortfolioContext : DbContext
    {
        public PortfolioContext() { }
        private const string connect = "Server=198.211.29.93,1433;Database=PortfolioDB;User Id=sa;Password='Apple&Pie79';MultipleActiveResultSets=true;Persist Security Info=True;";
        private static readonly DbContextOptions<PortfolioContext> options =
            new DbContextOptionsBuilder<PortfolioContext>().UseSqlServer(connect).Options;

        private static readonly Lazy<PortfolioContext> lazy = new Lazy<PortfolioContext>(() => new PortfolioContext(options));

        public static PortfolioContext Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public PortfolioContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(connect);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectCreator> Users { get; set; }

        public DbSet<ProjectLink> Links { get; set; }

        public DbSet<PublishedHistory> ProjectPublishHistory { get; set; }

        public DbSet<ProjectRequirement> ProjectRequirements { get; set; }

        public DbSet<Experience> Experiences { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Degree>()
            .ToTable("MyDegrees");

            // PROJECT
            modelBuilder.Entity<Project>()
                .HasMany(p => p.ProjectRequirements)
                .WithOne(pr => pr.Project)
                .OnDelete(DeleteBehavior.Cascade)
                ;

            modelBuilder.Entity<Project>()
            .Property(b => b.Started)
            .HasColumnName("SDate");

            modelBuilder.Entity<Project>()
            .Property(b => b.Completed)
            .HasColumnName("CDate");

            modelBuilder.Entity<ProjectRequirement>()
                .HasOne(pr => pr.Project)
                .WithMany(p => p.ProjectRequirements)
                .OnDelete(DeleteBehavior.Restrict)
                ;

            modelBuilder.Entity<Project>()
                .HasMany(p => p.ProjectLinks)
                .WithOne(pl => pl.Project)
                .OnDelete(DeleteBehavior.Cascade)
                ;

            modelBuilder.Entity<ProjectLink>()
                .HasOne(pl => pl.Project)
                .WithMany(p => p.ProjectLinks)
                .OnDelete(DeleteBehavior.Restrict)
                ;

            // modelBuilder.Entity<Project>()
            //     .HasMany(p => p.ProjectHistory)
            //     .WithOne(ph => ph.Project)
            //     .OnDelete(DeleteBehavior.Cascade)
            //     ;

            // modelBuilder.Entity<PublishedHistory>()
            //     .HasOne(ph => ph.Project)
            //     .WithMany(p => p.ProjectHistory)
            //     .OnDelete(DeleteBehavior.Restrict)
            //     ;

            modelBuilder.Entity<ProjectCreator>()
                .HasMany(pc => pc.Projects)
                .WithOne(p => p.ProjectOwner)
                .OnDelete(DeleteBehavior.Cascade)
                ;

            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectOwner)
                .WithMany(po => po.Projects)
                .OnDelete(DeleteBehavior.Restrict)
                ;

            modelBuilder.Entity<ProjectCreator>()
                .HasMany(pc => pc.Experiences)
                .WithOne(exp => exp.ProjectCreator)
                .OnDelete(DeleteBehavior.Cascade)
                ;

            modelBuilder.Entity<Experience>()
                .HasOne(exp => exp.ProjectCreator)
                .WithMany(pc => pc.Experiences)
                .OnDelete(DeleteBehavior.Cascade)
                ;
            modelBuilder.Entity<Experience>()
                .Property(b => b.State)
                .HasColumnName("MyState");

            modelBuilder.Entity<Experience>()
                .Property(b => b.Started)
                .HasColumnName("SDate");

                modelBuilder.Entity<Experience>()
                .Property(b => b.Completed)
                .HasColumnName("CDate");

            modelBuilder.Entity<ProjectCreator>()
                .HasMany(pc => pc.Certifications)
                .WithOne(c => c.ProjectCreator)
                .OnDelete(DeleteBehavior.Cascade)
                ;

            modelBuilder.Entity<Certification>()
                .HasOne(c => c.ProjectCreator)
                .WithMany(pc => pc.Certifications)
                .OnDelete(DeleteBehavior.Restrict)
                ;

            modelBuilder.Entity<ProjectCreator>()
                .HasMany(pc => pc.Degrees)
                .WithOne(d => d.ProjectCreator)
                .OnDelete(DeleteBehavior.Cascade)
                ;

            modelBuilder.Entity<Degree>()
                .HasOne(d => d.ProjectCreator)
                .WithMany(pc => pc.Degrees)
                .OnDelete(DeleteBehavior.Restrict)
                ;
            modelBuilder.Entity<Degree>()
                .Property(d => d.State)
                .HasColumnName("MyState");
                // modelBuilder.Entity<Degree>()
                // .Property(d => d.GraduationYear)
                // .HasColumnType("DateTime");
            // Experience

            modelBuilder.Entity<Experience>()
                .HasMany(exp => exp.Roles)
                .WithOne(r => r.Experience)
                .OnDelete(DeleteBehavior.Cascade)
                ;

            modelBuilder.Entity<Role>()
                .HasOne(r => r.Experience)
                .WithMany(exp => exp.Roles)
                .OnDelete(DeleteBehavior.Cascade)
                ;

            // EDUCATION

            base.OnModelCreating(modelBuilder);
        }
    }
}
