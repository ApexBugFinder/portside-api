using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Portfolio.PorfolioDomain.Core.Helpers;


namespace Portfolio.PorfolioDomain.Core.Entities
{
[Table("ProjectCreators")]
public class ProjectCreator
{
    [Key]
    [Required]
    [MaxLength(70)]
    public string SubjectId {get; set;}
    private string message = "";

        public List<Project> Projects { get; set; }

        public List<Experience> Experiences { get; set; }

        public List<Certification> Certifications { get; set; }

        public List<Degree> Degrees { get; set; }

        public string userPicUrl { get; set; }

        public string Username {get; set;}
        public ProjectCreator()
        {
            this.Projects = new List<Project>();
            this.Experiences = new List<Experience>();
            this.Certifications = new List<Certification>();
            this.Degrees = new List<Degree>();
        }

        public void Print()
        {
            message = "ProjectCreator Object Print Report"
                + "\nProjectCreator ID: " + this.SubjectId
                + "\nProjectCreator Username: " + this.Username
                + "\nProject Creator has " + Projects.Count + " and then are: ";

            for (int i=0; i <= Projects.Count; i++)
            {
                message += "\nProjectID: " + Projects[i].ID + ", ProjectName: " + Projects[i].ProjectName;
            }
            Notification.PostMessage(message);
        }
    }
}
