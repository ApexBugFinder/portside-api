using Portfolio.PorfolioDomain.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Portfolio.PorfolioDomain.Core.Entities
{
    public class ProjectLink
    {
        private string message;
        [Key]
        [Required]
    [MaxLength(70)]
        public string ID { get; set; }

        public string Link { get; set; }

        [Required]
        [ForeignKey("Project")]
    [MaxLength(70)]
        public string ProjectID { get; set; }
        public virtual Project Project { get; set; }

        public string Service { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }

        public ProjectLink()
        {
            message = "";
        }



        public void Print()
        {
            message = "ProjectLink Object Print Report"
                + "\nProjectLink ID: " + this.ID
                + "\nProjectLink ProjectId: " + this.ProjectID
                + "\nProjectLinks Requirement: " + this.Service
                ;


            Notification.PostMessage(message);




        }
    }
}
