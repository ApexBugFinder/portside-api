
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Portfolio.PorfolioDomain.Core.Entities;
using System.Threading.Tasks;
namespace Portfolio.WebApp.Helpers {
  public class SqlReaderProject {

    private string message = "";

    public SqlDataReader reader {get; set;}
    private ProjectCreator projectCreator { get; set;}

    private Project project {get; set;}
    private ProjectRequirement projectRequirement  {get; set;}
    private ProjectLink projectLink {get; set;}
    private Experience myExperience { get; set; }
    private Role myRole { get; set; }
    private Certification myCertification { get; set; }
    private Degree myDegree  { get; set; }

    private List<Project> results { get; set;  }


    public SqlReaderProject() {
      results = new List<Project>();
    }
    public async Task<List<Project>> Getdata (SqlDataReader dataReader) {

      this.reader = dataReader;

 using (this.reader)
{
      if (this.reader.HasRows)
      {
              while (this.reader.Read())
              {












                    Project project = new Project();
                    ProjectRequirement projectRequirement = new ProjectRequirement();
                    ProjectLink projectLink = new ProjectLink();


                //     // IF I HAVE PROJECT CREATOR CHECK FOR PROJECTS
                if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)))
                || reader.GetValue(0) != System.DBNull.Value)
                {

                  message = "ProjectID: " + ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0));
                Notification.PostMessage(message);
                  project = new Project()
                  {
                    ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)) ,
                    ProjectName = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ProjectName"))),
                    ProjectCreatorID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ProjectCreatorID"))),
                    Description = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Description"))),
                    Started = (DateTime)ConvertDBVal.ConvertFromDBVal<DateTime>(reader.GetValue(reader.GetOrdinal("SDate"))),
                    Completed = (DateTime)ConvertDBVal.ConvertFromDBVal<DateTime>(reader.GetValue(reader.GetOrdinal("CDate"))),
                    Banner = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Banner"))),
                    Published = (Boolean)ConvertDBVal.ConvertFromDBVal<Boolean>(reader.GetValue(reader.GetOrdinal("Published")))

                  };

                  // message = "LinkID: " + ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("LinkID")));
                  // Notification.PostMessage(message);
                  // IF I HAVE PROJECTS CHECK FOR PROJECT LINKS
                  if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("LinkID")))) || reader.GetValue(reader.GetOrdinal("LinkID")) != System.DBNull.Value)
                  {
                    projectLink = new ProjectLink()
                    {
                      ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("LinkID"))),
                      ProjectID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)),
                      Link = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Link"))),
                      Service = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Service"))),
                      Description = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("pl_Description"))),
                      Title = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Title")))
                    };
                  }

                  // IF I HAVE PROJECTS CHECK FOR PROJECT REQUIREMENTS
                  if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("RequirementID")))) || reader.GetValue(reader.GetOrdinal("RequirementID")) != System.DBNull.Value)
                  {
                    projectRequirement = new ProjectRequirement()
                    {
                      ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("RequirementID"))),
                      ProjectID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)),
                      Requirement = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Requirement")))
                    };
                  }
                }



                // message = "Count of Users PointA: " + results.Count;
                // Notification.PostMessage(message);





                  // CHECK FOR USER PROJECTS, CERTS, DEGREES, EXPERIENCES
                  // IF PROJECTS EXXISTED PREVIOUSLY
                  //     // IF PROJECT EXISTS PREVIOUSLY

                  if (!String.IsNullOrEmpty(project.ID) && results.Exists(i => i.ID == project.ID))
                  {
                    // CHECK PRS
                    // message = "Reached Projects - ProjectRequirements Merge";
                    // Notification.PostMessage(message);
                    if (!String.IsNullOrEmpty(projectRequirement.ID) &&
                    results.Find(i => i.ID == project.ID).ProjectRequirements.Exists(i => i.ID == projectRequirement.ID))
                    {
                      // DO NOTHING
                    }
                    else
                    {
                      // ADD PR

                      // message = "Reached ProjectRequirements Merge PR";
                      // Notification.PostMessage(message);
                      results.Find(b => b.ID == projectRequirement.ProjectID).ProjectRequirements.Add(projectRequirement);
                    }
                    // message = "Reached Projects - ProjectLinks Merge";
                    // Notification.PostMessage(message);
                    //   // CHECK PLS
                    if (!String.IsNullOrEmpty(projectLink.ID) && results.Find(i => i.ID == project.ID).ProjectLinks.Exists(i => i.ID == projectLink.ID))
                    {
                      // DO NOTHING
                    }
                    else
                    {
                      // ADD PL
                      if (projectLink.ID != "null")
                      results.Find(b => b.ID == projectLink.ProjectID).ProjectLinks.Add(projectLink);
                    }



                  } else {
                  // message = "New User";
                  // Notification.PostMessage(message);
                  if(!String.IsNullOrEmpty(projectLink.ID) && !(project.ProjectLinks.Contains(projectLink)) && (projectLink.ID != "null")) {
                        project.ProjectLinks.Add(projectLink);
                  }


                  if(!String.IsNullOrEmpty(projectRequirement.ID) || !(project.ProjectRequirements.Contains(projectRequirement)))
                  project.ProjectRequirements.Add(projectRequirement);

                  if(!String.IsNullOrEmpty(project.ID))
                  results.Add(project);

                  GC.Collect();
                  // message = "Count of Users PointB: " + results.Count;
                  // Notification.PostMessage(message);
                }
              } // CLOSE WHILE
        }

      } // CLOSE USING

      GC.Collect();


      return results;
    }
  }
}
