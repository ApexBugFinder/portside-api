
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Portfolio.PorfolioDomain.Core.Entities;
using System.Threading.Tasks;
namespace Portfolio.WebApp.Helpers
{
  public class SqlReaderProjectLink
  {

    private string message = "";

    public SqlDataReader reader { get; set; }

    private ProjectLink projectLink { get; set; }

    private List<ProjectLink> results { get; set; }


    public SqlReaderProjectLink()
    {
      results = new List<ProjectLink>();
    }
    public async Task<List<ProjectLink>> Getdata(SqlDataReader dataReader)
    {

      this.reader = dataReader;

      await using (this.reader)
      {
        if (this.reader.HasRows)
        {
          while (this.reader.Read())
          {




            ProjectLink projectLink = new ProjectLink();










            message = "ProjectID: " + ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0));
            Notification.PostMessage(message);
            //     // IF I HAVE PROJECT CREATOR CHECK FOR PROJECTS


              message = "LinkID: " + ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0));
              Notification.PostMessage(message);
              // IF I HAVE PROJECTS CHECK FOR PROJECT LINKS
              if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0))) || reader.GetValue(0) != System.DBNull.Value)
              {
                projectLink = new ProjectLink()
                {
                  ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)),
                  ProjectID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ProjectID"))),
                  Link = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Link"))),
                  Service = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Service"))),
                  Title = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Title"))),
                  Description = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Description")))
                };
              }





            message = "Count of Users PointA: " + results.Count;
            Notification.PostMessage(message);


            //  IF PC EXISTS PREVIOUSLY  ====MERGE====


            // CHECK FOR USER PROJECTS, CERTS, DEGREES, EXPERIENCES
            // IF PROJECTS EXXISTED PREVIOUSLY
            //     // IF PROJECT EXISTS PREVIOUSLY


              message = "Reached Projects - ProjectLinks Merge";
              Notification.PostMessage(message);
              //   // CHECK PLS
              if (!String.IsNullOrEmpty(projectLink.ID) && results.Exists(i => i.ID == projectLink.ID))
              {
                // DO NOTHING
              }
              else
              {
                // ADD PR
                results.Add(projectLink);
              }





          } // CLOSE WHILE
        }

      } // CLOSE USING


      GC.Collect();
      return results;
    }
  }
}
