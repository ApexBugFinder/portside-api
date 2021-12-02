using Portfolio.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Dtos
{
    public class CertificationDto
    {
        private string message;


        public string ID { get; set; }


        public string ProjectCreatorID { get; set; }
        // public ProjectCreatorDto ProjectCreator { get; set; }

        public string CertName { get; set; }
        public bool IsActive { get; set; }

        public string CertID { get; set; }
        public string IssuingBody_Name { get; set; }
        public string IssuingBody_Logo { get; set; }

        public CertificationDto()
        {
            message = "";
        }
        public void Print()
        {
            message = "CertificationDto Object Print Report"
                + "\nCertificationDto ID: " + this.ID
                + "\nCertificationDto ProjectCreatorId: " + this.ProjectCreatorID
                + "\nCertificationDto IsActive: " + this.IsActive
                + "\nCertificationDto CertID: " + this.CertID
                + "\nCertificationDto IssuingBody_Name: " + this.IssuingBody_Name
                + "\nCertificationDto IssuingBody_Logo: " + this.IssuingBody_Logo


                ;


            Notification.PostMessage(message);

        }
    }
}
