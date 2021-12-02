using Portfolio.PorfolioDomain.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Portfolio.PorfolioDomain.Core.Entities
{

[Table("MyDegrees")]
    public class Degree
    {
        private string message;

        [Key]
        [Required]
        [MaxLength(70)]
        public string ID { get; set; }

        public string DegreeType { get; set; }

        [MaxLength(70)]
        [ForeignKey("ProjectCreator")]
        public string ProjectCreatorID { get; set; }

        public ProjectCreator ProjectCreator { get; set; }

        public string DegreeName { get; set; }

        public string Minors { get; set; }

        public string Institution { get; set; }

        public string InstitutionLogo {get; set; }

        public string City { get; set; }

        [Column("MyState")]
        public string State { get; set; }

        public bool Graduated { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime GraduationYear { get; set; }

        public Degree()
        {
            message = "";
        }
        public void Print()
        {
            message = "Degree Object Print Report"
                + "\nDegree ID: " + this.ID
                + "\nDegree ProjectCreatorId: " + this.ProjectCreatorID
                + "\nDegree DegreeName: " + this.DegreeName
                + "\nDegree Minors: " + this.Minors
                + "\nDegree Institution: " + this.Institution
                + "\nDegree City: " + this.City
                + "\nDegree State: " + this.State
                + "\nDegree Graduated: " + this.Graduated
                + "\nDegree GraduationYear: " + this.GraduationYear
                ;





            Notification.PostMessage(message);




        }
    }
}
