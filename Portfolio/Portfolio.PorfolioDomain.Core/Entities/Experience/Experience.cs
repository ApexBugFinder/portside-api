using Portfolio.PorfolioDomain.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Portfolio.PorfolioDomain.Core.Entities
{
    public class Experience
    {
        private string message = "";

    [Key]
    [Required]
    [MaxLength(70)]
        public string ID { get; set; }

        [ForeignKey("ProjectCreator")]
        [MaxLength(70)]
        public string ProjectCreatorID { get; set; }
        public virtual ProjectCreator ProjectCreator { get; set; }


[MaxLength(200)]
    public string Company { get; set; }

    [MaxLength(200)]
        public string Title { get; set; }

        public string LogoUrl { get; set; }

        [Column("SDate")]
        public DateTime Started { get; set; }

        [Column("CDate")]
        public DateTime Completed { get; set; }

    [MaxLength(200)]
        public string City { get; set; }

    [Column("MyState")]
    [MaxLength(40)]
        public string State { get; set; }

        public List<Role> Roles { get; set; }

        public Experience()
        {
            Roles = new List<Role>();
        }

        public void Print()
        {
            message = "Experience Object Print Report"
                + "\nExperience ID: " + this.ID
                + "\nExperience ProjectCreatorId: " + this.ProjectCreatorID
                + "\nExperience has " + Roles.Count + " Roles and then are: ";


            Notification.PostMessage(message);
            for (int i = 0; i <= Roles.Count; i++)
            {
               Roles[i].Print();
            }



        }
    }
}
