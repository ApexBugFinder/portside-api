
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Portfolio.PorfolioDomain.Core.Entities;
using System.Threading.Tasks;
namespace Portfolio.WebApp.Helpers
{
  public class SqlReaderRole
  {

    private string message = "";

    public SqlDataReader reader { get; set; }


    private Role myRole { get; set; }

    private List<Role> results {get; set;}


    public SqlReaderRole()
    {
      results = new List<Role>();
    }
    public async Task<List<Role>> Getdata(SqlDataReader dataReader)
    {

      this.reader = dataReader;

      await using (this.reader)
      {
        if (this.reader.HasRows)
        {
          while (this.reader.Read())
          {




            Role myRole = new Role();








              // IF I HAVE Roles






                message = "RoleID: " + ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0));
                Notification.PostMessage(message);
                
                if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)))
                || reader.GetValue(0) != System.DBNull.Value)
                {
                  myRole = new Role()
                  {
                    ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)),
                    ExperienceID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ExperienceID"))),
                    MyTitle = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("MyTitle"))),
                    MyRole = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("MyRole")))
                  };
                }




              message = "Count of Users PointA: " + results.Count;
              Notification.PostMessage(message);



                // // ==================
                // // IF EXPERIENCE EXISTS
                Role addedUserRole = new Role();
                if (results.Exists(i => i.ID == myRole.ID))
                {

                    //  DO NOTHING

                }
                else
                {
                      message = "New Role";
                      Notification.PostMessage(message);

                      results.Add(myRole);
                      message = "Count of Users PointB: " + results.Count;
                      Notification.PostMessage(message);
                }
            }
          }





















        }



        return results;
      }
    }
  }
