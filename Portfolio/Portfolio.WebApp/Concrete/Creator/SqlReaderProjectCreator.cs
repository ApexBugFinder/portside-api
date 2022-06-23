
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Portfolio.PorfolioDomain.Core.Entities;
using System.Threading.Tasks;
namespace Portfolio.WebApp.Helpers
{
  public class SqlReaderProjectCreator
  {

    private string message = "";

    public SqlDataReader reader { get; set; }
    private ProjectCreator projectCreator { get; set; }

    private Project project { get; set; }
    private ProjectRequirement projectRequirement { get; set; }
    private ProjectLink projectLink { get; set; }
    private Experience myExperience { get; set; }
    private Role myRole { get; set; }
    private Certification myCertification { get; set; }
    private Degree myDegree { get; set; }

    private List<ProjectCreator> results { get; set; }


    public SqlReaderProjectCreator()
    {
      results = new List<ProjectCreator>();
    }
    public async Task<List<ProjectCreator>> Getdata(SqlDataReader dataReader)
    {

      this.reader = dataReader;

      await using (this.reader)
      {
        if (this.reader.HasRows)
        {
          while (this.reader.Read())
          {


            ProjectCreator projectCreator = new ProjectCreator();
            Project project = new Project();
            ProjectRequirement projectRequirement = new ProjectRequirement();
            ProjectLink projectLink = new ProjectLink();
            Experience myExperience = new Experience();
            Role myRole = new Role();
            Certification myCertification = new Certification();
            Degree myDegree = new Degree();




            // CHECK FOR PROJECT CREATOR
            if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<String>(reader.GetValue(0)).ToString()) ||
            reader.GetValue(0) != System.DBNull.Value)
            {



              projectCreator = new ProjectCreator()
              {
                SubjectId = reader.GetValue(0).ToString(),
                Username = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Username"))),
                userPicUrl = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("userPicUrl")))


              };

              // message = "User data: " + projectCreator.SubjectId
              // + "\t" + projectCreator.userPicUrl
              // + "\t" + projectCreator.Username
              // ;
              // Notification.PostMessage(message);
              // CHECK IF PREVIOUSLY ADDED USER



              // IF I HAVE PROJECT CREATOR CHECK FOR EXPERIENCES
              // message = "ExperienceID: " + ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ExperienceID")));
              // Notification.PostMessage(message);


              if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ExperienceID"))))
              && reader.GetValue(reader.GetOrdinal("ExperienceID")) != System.DBNull.Value)
              {
                myExperience = new Experience()
                {
                  ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ExperienceID"))),
                  ProjectCreatorID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)),
                  Company = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Company"))),
                  Title = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Title"))),
                  LogoUrl = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("LogoUrl"))),
                  Started = (DateTime)ConvertDBVal.ConvertFromDBVal<DateTime>(reader.GetValue(reader.GetOrdinal("ExperienceSDate"))),
                  Completed = (DateTime)ConvertDBVal.ConvertFromDBVal<DateTime>(reader.GetValue(reader.GetOrdinal("ExperienceCDate"))),
                  City = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ExperienceCity"))),
                  State = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ExperienceState")))
                };

                //       // IF I HAVE EXPERIENCE CHECK FOR ROLE
                // message = "RoleID: " + ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("RoleID")));
                // Notification.PostMessage(message);
                if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("RoleID"))))
                || reader.GetValue(reader.GetOrdinal("RoleID")) != System.DBNull.Value)
                {
                  myRole = new Role()
                  {
                    ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("RoleID"))),
                    ExperienceID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ExperienceID"))),
                    MyTitle = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("MyTitle"))),
                    MyRole = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("MyRole")))
                  };
                }
              }


              // message = "ProjectID: " + ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ProjectID")));
              // Notification.PostMessage(message);
              //     // IF I HAVE PROJECT CREATOR CHECK FOR PROJECTS
              if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ProjectID"))))
              || reader.GetValue(reader.GetOrdinal("ProjectID")) != System.DBNull.Value)
              {
                project = new Project()
                {
                  ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(29)),
                  ProjectName = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ProjectName"))),
                  ProjectCreatorID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)),
                  Description = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Description"))),
                  Started = (DateTime)ConvertDBVal.ConvertFromDBVal<DateTime>(reader.GetValue(reader.GetOrdinal("ProjectSDate"))),
                  Completed = (DateTime)ConvertDBVal.ConvertFromDBVal<DateTime>(reader.GetValue(reader.GetOrdinal("ProjectCDate"))),
                  Banner = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Banner"))),
                  Published = (Boolean)ConvertDBVal.ConvertFromDBVal<Boolean>(reader.GetValue(reader.GetOrdinal("Published")))

                };

                // message = "LinkID: " + ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("LinkID")));
                // Notification.PostMessage(message);
                // IF I HAVE PROJECTS CHECK FOR PROJECT LINKS
                if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("LinkID")))) && reader.GetValue(reader.GetOrdinal("LinkID")) != System.DBNull.Value)
                {
                  projectLink = new ProjectLink()
                  {
                    ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("LinkID"))),
                    ProjectID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ProjectID"))),
                    Link = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Link"))),
                    Service = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Service"))),
                    Title = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("pl_Title"))),
                    Description = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("pl_Description")))
                  };
                }

                // IF I HAVE PROJECTS CHECK FOR PROJECT REQUIREMENTS
                if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("RequirementID")))) || reader.GetValue(reader.GetOrdinal("RequirementID")) != System.DBNull.Value)
                {
                  projectRequirement = new ProjectRequirement()
                  {
                    ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("RequirementID"))),
                    ProjectID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ProjectID"))),
                    Requirement = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Requirement")))
                  };
                }
              }


              //     // CHECK FOR CERTIFICATIONS
              if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("CertIndID")))) || reader.GetValue(reader.GetOrdinal("CertIndID")) != System.DBNull.Value)
              {
                myCertification = new Certification()
                {
                  ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("CertIndID"))),
                  ProjectCreatorID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)),
                  CertName = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("CertName"))),
                  IsActive = (Boolean)ConvertDBVal.ConvertFromDBVal<Boolean>(reader.GetValue(reader.GetOrdinal("IsActive"))),
                  CertID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("CertID"))),
                  IssuingBody_Logo = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("IssuingBody_Logo"))),
                  IssuingBody_Name = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("IssuingBody_Name")))
                };
              }
              //     // CHECK FOR DEGREES
              if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("DegreeID")))) || reader.GetValue(reader.GetOrdinal("DegreeID")) != System.DBNull.Value)
              {
                myDegree = new Degree()
                {
                  ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("DegreeID"))),
                  DegreeType = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("DegreeType"))),
                  ProjectCreatorID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)),
                  Minors = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Minors"))),
                  Institution = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Institution"))),
                  InstitutionLogo = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("InstitutionLogo"))),
                  City = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("DegreeCity"))),
                  State = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("DegreeState"))),
                  Graduated = (Boolean)ConvertDBVal.ConvertFromDBVal<Boolean>(reader.GetValue(reader.GetOrdinal("Graduated"))),
                  // GraduationYear = ConvertDBVal.ConvertFromDBVal<DateTime>(reader.GetValue(reader.GetOrdinal("GraduationYear")))
                };
              }



              // message = "Count of Users PointA: " + results.Count;
              // Notification.PostMessage(message);
              // Notification.PostMessage("PC ID: " + projectCreator.SubjectId);
              // Notification.PostMessage("ProjectID: " + project.ID);
              // Notification.PostMessage("ProjectRequirement ID: " + projectRequirement.ID);
              // Notification.PostMessage("ProjectLink ID: " + projectLink.ID);
              // Notification.PostMessage("ExperienceID: " + myExperience.ID);
              // Notification.PostMessage("RoleID " + myRole.ID);
              // Notification.PostMessage("Cert ID: " + myCertification.ID);
              // Notification.PostMessage("DegreeID: " + myDegree.ID);


              //  IF PC EXISTS PREVIOUSLY  ====MERGE====
              if (results.Exists(p => p.SubjectId == projectCreator.SubjectId))
              {
                // message = "User was previously on the table: " + project.ID;
                // Notification.PostMessage(message);

                // CHECK FOR USER PROJECTS, CERTS, DEGREES, EXPERIENCES
                // IF PROJECTS EXXISTED PREVIOUSLY
                //     // IF PROJECT EXISTS PREVIOUSLY

                if (!String.IsNullOrEmpty(project.ID) && results.Find(i => i.SubjectId == projectCreator.SubjectId).Projects.Exists(i => i.ID == project.ID))
                {
                  // CHECK PRS
                  // message = "Reached Projects - ProjectRequirements Merge";
                  // Notification.PostMessage(message);
                  if (!String.IsNullOrEmpty(projectRequirement.ID) &&
                  results.Find(i => i.SubjectId == projectCreator.SubjectId).Projects.Find(i => i.ID == project.ID).ProjectRequirements.Exists(i => i.ID == projectRequirement.ID))
                  {
                    // DO NOTHING
                  }
                  else
                  {
                    // ADD PROJ

                    // message = "Reached ProjectRequirements Merge PR";
                    // Notification.PostMessage(message);
                    results.Find(i => i.SubjectId == projectCreator.SubjectId).Projects.Find(b => b.ID == project.ID).ProjectRequirements.Add(projectRequirement);
                  }
                  // message = "Reached Projects - ProjectLinks Merge";
                  // Notification.PostMessage(message);
                  //   // CHECK PLS
                  if (!String.IsNullOrEmpty(project.ID) && results.Find(i => i.SubjectId == projectCreator.SubjectId).Projects.Find(i => i.ID == project.ID).ProjectLinks.Exists(i => i.ID == projectLink.ID))
                  {
                    // DO NOTHING
                  }
                  else
                  {
                    // ADD PL
                    results.Find(i => i.SubjectId == projectCreator.SubjectId).Projects.Find(b => b.ID == project.ID).ProjectLinks.Add(projectLink);
                  }



                }
                else
                {

                  // message = "Reached Projects - New Project Merge";
                  // Notification.PostMessage(message);

                  if (!String.IsNullOrEmpty(projectRequirement.ID)
                  // && !(results.Find(i => i.SubjectId == projectCreator.SubjectId).Projects.Find(i => i.ID == project.ID).ProjectRequirements.Contains(projectRequirement))
                  )
                  {
                    project.ProjectRequirements.Add(projectRequirement);
                  }
                  if (!String.IsNullOrEmpty(projectLink.ID)
                  // && !(results.Find(i => i.SubjectId == projectCreator.SubjectId).Projects.Find(i => i.ID == project.ID).ProjectLinks.Contains(projectLink))
                  )
                  {
                    project.ProjectLinks.Add(projectLink);
                  }
                  results.Find(i => i.SubjectId == projectCreator.SubjectId).Projects.Add(project);
                }



                // message = "Reached Experiences: ID -" + myExperience.ID;
                // Notification.PostMessage(message);
                // // ==================
                // // IF EXPERIENCE EXISTS

                if (!String.IsNullOrEmpty(myExperience.ID) && results.Find(i => i.SubjectId == projectCreator.SubjectId).Experiences.Exists(i => i.ID == myExperience.ID))
                {

                  // DO NOTHING TO EXPERIENCES
                  //  CHECK ROLES
                  if (!String.IsNullOrEmpty(myRole.ID) && !results.Find(i => i.SubjectId == projectCreator.SubjectId).Experiences.Find(i => i.ID == myExperience.ID).Roles.Exists(i => i.ID == myRole.ID))
                  {
                    results.Find(i => i.SubjectId == projectCreator.SubjectId).Experiences.Find(b => b.ID == myExperience.ID).Roles.Add(myRole);
                  }



                }
                else
                {
                  if (!String.IsNullOrEmpty(myRole.ID) && myRole.ExperienceID == myExperience.ID)
                    myExperience.Roles.Add(myRole);

                  results.Find(i => i.SubjectId == projectCreator.SubjectId).Experiences.Add(myExperience);




                }
                // // ===============
                // // DEGREES
                if (!String.IsNullOrEmpty(myDegree.ID) && !results.Find(i => i.SubjectId == projectCreator.SubjectId).Degrees.Exists(i => i.ID == myDegree.ID))
                  results.Find(i => i.SubjectId == projectCreator.SubjectId).Degrees.Add(myDegree);



                // ===========
                // CERTS
                if (!String.IsNullOrEmpty(myCertification.ID) && !results.Find(i => i.SubjectId == projectCreator.SubjectId).Certifications.Exists(i => i.ID == myCertification.ID))
                  results.Find(i => i.SubjectId == projectCreator.SubjectId).Certifications.Add(myCertification);


              }// CLOSE MERGE
              else
              {
                // message = "New User";
                // Notification.PostMessage(message);
                results.Add(projectCreator);
                // message = "Count of Users PointB: " + results.Count;
                // Notification.PostMessage(message);
              }
            }
          }





















        }


        await this.reader.CloseAsync();
        GC.Collect();
        return results;
      }

    }
  }
}