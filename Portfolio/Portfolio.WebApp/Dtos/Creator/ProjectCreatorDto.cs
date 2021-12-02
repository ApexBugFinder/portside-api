using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Helpers;

namespace Portfolio.WebApp.Dtos
{
    public class ProjectCreatorDto
    {
        private string message = "";
        private readonly IMapper _mapper;
        public string ID { get; set; }

        public string Username { get; set; }

        public List<ProjectDto> Projects { get; set; }

        public List<ExperienceDto> Experiences {get; set; }

        public List<DegreeDto> Degrees {get; set; }

        public List <CertificationDto> Certifications {get; set; }

        public string UserPicUrl { get; set; }


        public ProjectCreatorDto()
        {
            this._mapper = new Helpers.MapperConfiguration().Configure();
            Projects = new List<ProjectDto>();
            Experiences = new List<ExperienceDto>();
            Degrees = new List<DegreeDto>();
            Certifications = new List<CertificationDto>();
        }

        public void Print()
        {
            message = "ProjectCreator Object Print Report"
                + "\nProjectCreator ID: " + this.ID
                + "\nProjectCreator ProjectId: " + this.Username
                + "\nProject Creator has " + Projects.Count + " and then are: ";

            for (int i = 0; i <= Projects.Count; i++)
            {
                message += "\nProjectID: " + Projects[i].ID + ", ProjectName: " + Projects[i].ProjectName;
            }
            Notification.PostMessage(message);




        }
    }
}
