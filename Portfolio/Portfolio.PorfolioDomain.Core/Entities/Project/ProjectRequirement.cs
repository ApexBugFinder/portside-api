using Portfolio.PorfolioDomain.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Portfolio.PorfolioDomain.Core.Entities
{
    public class ProjectRequirement
    {
        private string message = "";
        [Key]
        [Required]
        [MaxLength(70)]
        public string ID { get; set; }


        [ForeignKey("Project")]
        [MaxLength(70)]
        public string ProjectID { get; set; }        public virtual Project Project { get; set; }

        public string Requirement{ get; set; }

        public ProjectRequirement()
        {
        }

        public void Print()
        {
            message = "ProjectRequirements Object Print Report"
                + "\nProjectRequirement ID: " + this.ID
                + "\nProjectRequirement ProjectId: " + this.ProjectID
                + "\nProjectRequirements Requirement: " + this.Requirement
                ;

            Notification.PostMessage(message);
        }
    }
}
