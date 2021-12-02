
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Portfolio.PorfolioDomain.Core.Entities;
using System.Threading.Tasks;
namespace Portfolio.WebApp.Helpers {
  public class SqlReaderProjectRequirement {

    private string message = "";

    public SqlDataReader reader {get; set;}
    private ProjectRequirement projectRequirement  {get; set;}


    private List<ProjectRequirement> results { get; set;  }


    public SqlReaderProjectRequirement() {
      results = new List<ProjectRequirement>();
    }
    public async Task<List<ProjectRequirement>> Getdata (SqlDataReader dataReader) {

      this.reader = dataReader;

 await using (this.reader)
{
      if (this.reader.HasRows)
      {
              while (this.reader.Read())
              {




              ProjectRequirement projectRequirement = new ProjectRequirement();













                  // IF I HAVE PROJECTS CHECK FOR PROJECT REQUIREMENTS
                  if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0))) || reader.GetValue(0) != System.DBNull.Value)
                  {
                    projectRequirement = new ProjectRequirement()
                    {
                      ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)),
                      ProjectID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ProjectID"))),
                      Requirement = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Requirement")))
                    };
                  }




                // message = "Count of Users PointA: " + results.Count + "ProjectReqID: " +projectRequirement.ID;
                // Notification.PostMessage(message);


                //  IF PRs EXISTS PREVIOUSLY  ====MERGE====



                    // message = "Reached Projects - ProjectRequirements Merge";
                    // Notification.PostMessage(message);
                    if (!String.IsNullOrEmpty(projectRequirement.ID) && results.Exists(i => i.ID == projectRequirement.ID))
                    {
                      // DO NOTHING
                    }
                    else
                    {
                      // ADD PR
                      results.Add(projectRequirement);
                      // message = "Reached ProjectRequirements Merge PR";
                      // Notification.PostMessage(message);

                    }




              } // CLOSE WHILE
        }

      } // CLOSE USING



      return results;
    }
  }
}
