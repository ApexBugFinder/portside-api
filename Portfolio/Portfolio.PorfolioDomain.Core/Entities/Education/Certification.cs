using Portfolio.PorfolioDomain.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Portfolio.PorfolioDomain.Core.Entities
{
    public class Certification
    {
        private string message;

        [Key]
        [Required]
        [MaxLength(70)]
        public string ID { get; set; }

        [MaxLength(70)]
        [ForeignKey("ProjectCreator")]
        public string ProjectCreatorID { get; set; }
        public ProjectCreator ProjectCreator { get; set; }

        public string CertName { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(70)]
        public string CertID { get; set; }
        public string IssuingBody_Name { get; set; }
        public string IssuingBody_Logo { get; set; }

        public Certification()
        {
            message = "";
        }
        public void Print()
        {
            message = "Certification Object Print Report"
              + "\nCertification ID: " + this.ID
              + "\nCertification ProjectCreatorId: " + this.ProjectCreatorID
              + "\nCertification IsActive: " + this.IsActive
              + "\nCertification CertID: " + this.CertID
              + "\nCertification IssuingBody_Name: " + this.IssuingBody_Name
              + "\nCertification IssuingBody_Logo: " + this.IssuingBody_Logo


              ;


            Notification.PostMessage(message);




        }
    }
}
