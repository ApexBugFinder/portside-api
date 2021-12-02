using Portfolio.PorfolioDomain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Services
{
  public static class PortfolioContextExtensions
  {
    const string userID = "D8D32EA4-5F9D-4BE9-9535-AB69C3F0A112";
    const string projectID = "E8885DC5-998B-48DF-86B7-536EDEA56BD5";
    const string experienceID = "0b8fc1ce-0d85-4c53-96c6-3002e4f74479";
    public static void EnsureSeedDataForContext(this PortfolioContext context)
    {
      if (!context.Users.Any())
      {
        var Users = new List<ProjectCreator>()
                {
                     new ProjectCreator()
                {
                    SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    Username = "Frank",
                },

                new ProjectCreator()
                {
                    SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    Username = "Claire",
                },
                    new ProjectCreator
                    {
                        SubjectId = userID,
                        Username = "SeedUser",
                    },
                     new ProjectCreator
                    {
                        SubjectId = Guid.NewGuid().ToString(),
                        Username = "SeedUser2",
                    }, new ProjectCreator
                    {
                         SubjectId = Guid.NewGuid().ToString(),
                        Username = "SeedUser3",
                    },
                      new ProjectCreator
                    {
                        SubjectId = Guid.NewGuid().ToString(),
                        Username = "SeedUser4",
                    }, new ProjectCreator
                    {
                         SubjectId = Guid.NewGuid().ToString(),
                        Username = "SeedUser5",
                    }
                };

        context.Users.AddRange(Users);
        context.SaveChanges();
      }
      if (!context.Projects.Any())
      {
        var Projects = new List<Project>()
                {
                    new Project
                    {
                        ID = projectID,
                        ProjectName = "SeedProject",
                        ProjectCreatorID = userID,
                        Description = "Nice Seed if I do say so myself",
                        Banner = "../../../assets/images/pngs/techDoc_banner_large.png",
                        Published = false
                    }
                };
        context.Projects.AddRange(Projects);
        context.SaveChanges();
      }

      if (!context.Links.Any())
      {
        var Links = new List<ProjectLink>()
                {
                    new ProjectLink
                    {
                        ID = "7533CE16-DAC6-4C2A-9CE2-14E5DFD7334C",
                        Service = "site",
                        ProjectID = projectID,
                        Link = "seed.com",
                        Description = "Description of SITE",
                        Title = "Title of WEBSITE"
                    },
                     new ProjectLink
                    {
                        ID = "7533CE16-DAC6-4C2A-9CE2-14E5DFDBASS3",
                        Service = "git",
                        ProjectID = projectID,
                        Link = "seed.com",
                        Description = "Description of API",
                        Title = "Title of API"
                    }
                };
        context.Links.AddRange(Links);
        context.SaveChanges();
      }

      if (!context.ProjectRequirements.Any())
      {
        var reqs = new List<ProjectRequirement>()
                {
                    new ProjectRequirement
                    {
                        ID = "8406BB4C-FD6C-4222-BDF5-E5C33E133CC4",
                        ProjectID = projectID,
                        Requirement = "Seed Requirement"
                    },
                     new ProjectRequirement
                    {
                        ID = "8406BB4C-FD6C-4222-BDF5-E5C33E133CB5",
                        ProjectID = projectID,
                        Requirement = "Seed Requirement2"
                    },
                     new ProjectRequirement
                    {
                        ID = "8406BB4C-FD6C-4222-BDF5-E5C33E133CB6",
                        ProjectID = projectID,
                        Requirement = "Seed Requirement3"
                    }

                };
        context.ProjectRequirements.AddRange(reqs);
        context.SaveChanges();
      }

      if (!context.ProjectPublishHistory.Any())
      {
        var histories = new List<PublishedHistory>()
                {
                    new PublishedHistory
                    {
                        ID = "A891710F-E15E-4945-869A-4043977648BA",
                        ProjectID = projectID,
                        PublishedOn = new DateTime(2021, 04, 13),
                        UnPublishedOn = new DateTime(2021, 04, 14)
                    }
                };
        context.ProjectPublishHistory.AddRange(histories);
        context.SaveChanges();
      }



      if (!context.Certifications.Any())
      {
        var certs = new List<Certification>() {
                    new Certification
                    {
                        ID = "2F72FA2B-C7DE-4DF3-ABAA-5BE1BC1DA233",
                        ProjectCreatorID = userID,
                        IsActive = true,
                        CertID = "3A6B2A7A-DA8A-4ECE-B84B-ADC36825A55C",
                        CertName = "Seed Certification",
                        IssuingBody_Name = "Seed Associates",
                        IssuingBody_Logo = "www.seed.com/seed_logo.svg",
                    }
                };
        context.Certifications.AddRange(certs);
        context.SaveChanges();
      }

      if (!context.Degrees.Any())
      {
        var degrees = new List<Degree>()
                {
                    new Degree
                    {
                        ID = "027FFB78-CEA4-4AA3-8DDC-F9A280FEEE2C",
                        ProjectCreatorID = userID,
                        DegreeType = "MS",
                        DegreeName = "Seed Science",
                        Minors = "Seed Culture",
                        Institution = "Seed University",
                        City = "Seed City",
                        State = "Seedy",
                        Graduated = true,
                        GraduationYear = new DateTime(2009/07/01)
                    }
                };
        context.Degrees.AddRange(degrees);
        context.SaveChanges();
      }
      if (!context.Experiences.Any())
      {
        var exp = new List<Experience>()
                {
                    new Experience
                    {
                        ID ="027FFB78-CEA4-4AA3-8DDC-F9A280FEEB12",
                        ProjectCreatorID = userID,
                        Company = "Seed Company Inc.",
                        Title = "Lead Seed",
                        LogoUrl = "www.seed.com/seedLogo.svg",
                        Started = new DateTime(2021, 04, 13),
                        Completed = new DateTime(2021, 04, 14),
                        City = "Seed City",
                        State = "Little Seed",
                    }
                };
        context.Experiences.AddRange(exp);
        context.SaveChanges();
      }

      if (!context.Roles.Any())
      {
        var roles = new List<Role>()
                {
                    new Role
                    {
                        ID = "3E8D96C2-0A34-4A60-A0FD-F4AECD75D823",
                        ExperienceID = "027FFB78-CEA4-4AA3-8DDC-F9A280FEEB12",
                        MyRole = "Did a lot of Seeding",
                        MyTitle = "Seed 1"
                    },
                    new Role
                    {
                        ID = "6341988b-2227-41da-a368-d86d9d87782c",
                        ExperienceID = "027FFB78-CEA4-4AA3-8DDC-F9A280FEEB12",
                        MyRole = "Did a lot of Seeding",
                        MyTitle = "Seed 2"
                    },
                    new Role
                    {
                        ID = "68938fd5-e11a-40c3-928d-fcb1ac1958af",
                        ExperienceID ="027FFB78-CEA4-4AA3-8DDC-F9A280FEEB12",
                        MyRole = "Did a lot of Seeding",
                        MyTitle = "Seed 3"
                    }
                };
        context.Roles.AddRange(roles);
        context.SaveChanges();
      }
    }
  }
}
