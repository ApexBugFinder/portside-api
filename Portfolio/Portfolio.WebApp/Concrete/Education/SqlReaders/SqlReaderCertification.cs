
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Portfolio.PorfolioDomain.Core.Entities;
using System.Threading.Tasks;
namespace Portfolio.WebApp.Helpers {
  public class SqlReaderCertification {

    private string message = "";

    public SqlDataReader reader {get; set;}
 private Certification returnCert { get; set; }


    private List<Certification> results { get; set;  }


    public SqlReaderCertification() {
      results = new List<Certification>();
    }
    public async Task<List<Certification>> Getdata (SqlDataReader dataReader) {

      this.reader = dataReader;

 await using (this.reader)
{
      if (this.reader.HasRows)
      {
       while (this.reader.Read())
        {



          Certification myCertification = new Certification();





      // CHECK FOR CERTIFICATIONS
          if (!String.IsNullOrEmpty(ConvertDBVal.ConvertFromDBVal<String>(reader.GetValue(0)).ToString()) ||
          reader.GetValue(0) != System.DBNull.Value)
          {
            myCertification = new Certification()
              {
                ID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(0)),
                ProjectCreatorID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("ProjectCreatorID"))),
                CertName = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("CertName"))),
                IsActive = (Boolean)ConvertDBVal.ConvertFromDBVal<Boolean>(reader.GetValue(reader.GetOrdinal("IsActive"))),
                CertID = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("CertID"))),
                IssuingBody_Logo = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("IssuingBody_Logo"))),
                IssuingBody_Name = ConvertDBVal.ConvertFromDBVal<string>(reader.GetValue(reader.GetOrdinal("IssuingBody_Name")))
              };

          }



            //

            //     // CHECK FOR DEGREES




            message = "Count of Users PointA: " + results.Count;
            Notification.PostMessage(message);




                results.Add(myCertification);

            }// CLOSE MERGE

          }
        }
















GC.Collect();
return results;



      }




}
    }
