using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Portfolio.PorfolioDomain.Core.Helpers;

namespace Portfolio.PorfolioDomain.Core.Entities
{
    public class Project
    {
        private string message = "";
        [Key]
        [Required]
        [MaxLength(70)]
        public string ID { get; set; }

        [MaxLength(200)]
        public string ProjectName { get; set; }




    [MaxLength(70)]
    [ForeignKey("ProjectOwner")]
        public string ProjectCreatorID { get; set; }


        public virtual ProjectCreator ProjectOwner { get; set; }



        [MaxLength(700)]
        public string Description { get; set; }


        [Column("SDate")]
        public DateTime Started { get; set; }

        [Column("CDate")]
        public DateTime Completed { get; set; }

        public string Banner { get; set; }



        public Boolean Published { get; set;  }


        // COLLECTIONS

        public virtual List<ProjectRequirement> ProjectRequirements { get; set; } = new List<ProjectRequirement>();

        public virtual List<ProjectLink> ProjectLinks { get; set; } = new List<ProjectLink>();

        // public virtual List<PublishedHistory> ProjectHistory { get; set; } = new List<PublishedHistory>();




        // CONSTRUCTOR

        public Project()
        {
            this.Banner= "https://firebasestorage.googleapis.com/v0/b/portfolio-a7105.appspot.com/o/defaults%2Fsite%2FAsset%203.svg?alt=media&token=489a99e5-166c-454b-8702-fd19b14f3336";
            this.ProjectRequirements = new List<ProjectRequirement>();
            this.ProjectLinks = new List<ProjectLink>();
            // this.ProjectHistory = new List<PublishedHistory>();
          //  this.ProjectOwner = new ProjectCreator();
        }



        public void Print()
        {
            message = "Project Object Print Report"
                + "\nProject ID: " + this.ID
                + "\nProject ProjectId: " + this.ProjectName
                + "\nProject Description: " + this.Description
                + "\nProject Banner Url: " + this.Banner
                + "\nProject Started: " + this.Started
                + "\nProject Completed: " + this.Completed
                + "\nProject IsPublished: " + this.Published
                ;


            Notification.PostMessage(message);




        }

    }
}
