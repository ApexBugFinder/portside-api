
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Portfolio.PorfolioDomain.Core.Entities;
using System.Threading.Tasks;
namespace Portfolio.WebApp.Helpers
{
  public class SqlReaderExperience
  {

    private string message = "";

    public SqlDataReader reader { get; set; }

    private Experience myExperience { get; set; }
    private Role myRole { get; set; }

    private List<Experience> results {get; set;}


    public SqlReaderExperience()
    {
      results = new List<Experience>();
    }
    public async Task<List<Experience>> Getdata(SqlDataReader dataReader)
    {

      this.reader = dataReader;

      await using (this.reader)
      {
        if (this.reader.HasRows)
        {
          while (this.reader.Read())
          {



            Experience myExperience = new Experience();
            Role myRole = new Role();








              // IF I HAVE PROJECT CREATOR CHECK FOR EXPERIENCES
              // message = "ExperienceID: " + ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0));
              // Notification.PostMessage(message);


              if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)))
              || reader.GetValue(0) != System.DBNull.Value)
              {
                myExperience = new Experience()
                {
                  ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)),
                  ProjectCreatorID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ProjectCreatorID"))),
                  Company = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Company"))),
                  Title = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Title"))),
                  LogoUrl = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("LogoUrl"))),
                  Started = (DateTime)ConvertDBVal.ConvertFromDBVal<DateTime>(reader.GetValue(reader.GetOrdinal("SDate"))),
                  Completed = (DateTime)ConvertDBVal.ConvertFromDBVal<DateTime>(reader.GetValue(reader.GetOrdinal("CDate"))),
                  City = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("City"))),
                  State = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("MyState")))
                };

                //       // IF I HAVE EXPERIENCE CHECK FOR ROLE
                message = "RoleID: " + ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("RoleID")));
                Notification.PostMessage(message);
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




              // message = "Count of Users PointA: " + results.Count;
              // Notification.PostMessage(message);



                // // ==================
                // // IF EXPERIENCE EXISTS

                if (results.Exists(i => i.ID == myExperience.ID))
                {

                      // DO NOTHING TO EXPERIENCES
                      //  CHECK ROLES
                      if (!results.Find(i => i.ID == myExperience.ID).Roles.Exists(i => i.ID == myRole.ID))
                      {
                        results.Find(b => b.ID == myExperience.ID).Roles.Add(myRole);
                      }

                      else
                      {
                        if (!String.IsNullOrEmpty(myRole.ID))
                          myExperience.Roles.Add(myRole);

                        
                        results.Add(myExperience);
                      }

                } else
                    {
                      // message = "New Experience";
                      // Notification.PostMessage(message);
                      if (!String.IsNullOrEmpty(myRole.ID) && !(myExperience.Roles.Contains(myRole)))
                        myExperience.Roles.Add(myRole);
                      results.Add(myExperience);
                      // message = "Count of Users PointB: " + results.Count;
                      // Notification.PostMessage(message);
                    }
            }
          }





















        }


        GC.Collect();
        return results;
      }
    }
  }
}