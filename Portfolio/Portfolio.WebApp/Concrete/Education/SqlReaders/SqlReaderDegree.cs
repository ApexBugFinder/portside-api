
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Portfolio.PorfolioDomain.Core.Entities;
using System.Threading.Tasks;
namespace Portfolio.WebApp.Helpers {
  public class SqlReaderDegree {

    private string message = "";

    public SqlDataReader reader {get; set;}
 private Degree returnCert { get; set; }


    private List<Degree> results { get; set;  }


    public SqlReaderDegree() {
      results = new List<Degree>();
    }
    public async Task<List<Degree>> Getdata (SqlDataReader dataReader) {

      this.reader = dataReader;

 await using (this.reader)
{
      if (this.reader.HasRows)
      {
       while (this.reader.Read())
        {



          Degree myDegree = new Degree();





      // CHECK FOR CERTIFICATIONS
          if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<String>(reader.GetValue(0)).ToString()) ||
          reader.GetValue(0) != System.DBNull.Value)
          {
              myDegree = new Degree()
              {
                ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)),
                DegreeType = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("DegreeType"))),
                DegreeName = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("DegreeName"))),
                ProjectCreatorID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ProjectCreatorID"))),
                Minors = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Minors"))),
                Institution = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("Institution"))),
                InstitutionLogo = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("InstitutionLogo"))),
                City = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("City"))),
                State = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("MyState"))),
                Graduated = (Boolean)ConvertDBVal.ConvertFromDBVal<Boolean>(reader.GetValue(reader.GetOrdinal("Graduated"))),
                GraduationYear = (DateTime)ConvertDBVal.ConvertFromDBVal<DateTime>(reader.GetValue(reader.GetOrdinal("GraduationYear")))
              };

          }



            //

            //     // CHECK FOR DEGREES




            // message = "Count of Users PointA: " + results.Count;
            // Notification.PostMessage(message);




                results.Add(myDegree);

            }// CLOSE MERGE

          }
        }

















return results;



      }




}
    }
