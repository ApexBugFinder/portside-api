using Portfolio.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.WebApp.Dtos
{
    public class DegreeDto
    {
        private string message;


        public string ID { get; set; }

        public string DegreeType { get; set; }


        public string ProjectCreatorID { get; set; }


        public string DegreeName { get; set; }

        public string Minors { get; set; }

        public string Institution { get; set; }

        public string InstitutionLogo { get; set;}

        public string City { get; set; }
        public string State { get; set; }

        public bool Graduated { get; set; }

        public DateTime GraduationYear { get; set; }

        public DegreeDto()
        {
            message = "";
        }
        public void Print()
        {
            message = "DegreeDto Object Print Report"
                + "\nDegreeDto ID: " + this.ID
                + "\nDegreeDto ProjectCreatorId: " + this.ProjectCreatorID
                + "\nDegreeDto DegreeName: " + this.DegreeName
                + "\nDegreeDto Minors: " + this.Minors
                + "\nDegreeDto Institution: " + this.Institution
                + "\nDegreeDto City: " + this.City
                + "\nDegreeDto State: " + this.State
                + "\nDegreeDto Graduated: " + this.Graduated
                + "\nDegreeDto GraduationYear: " + this.GraduationYear
                ;





            Notification.PostMessage(message);
        }
    }
}
