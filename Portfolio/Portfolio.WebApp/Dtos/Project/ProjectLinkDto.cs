using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Portfolio.WebApp.Helpers;
using Portfolio.PorfolioDomain.Core.Entities;

namespace Portfolio.WebApp.Dtos
{
    public class ProjectLinkDto
    {
        private string message;
        public string ID { get; set; }
        public string Link { get; set; }
        public string ProjectID { get; set; }

        public string Title {get; set; }

        public string Description { get; set; }
        //     public virtual ProjectDto Project { get; set; }

        public string Service { get; set; }

        private DtoEntityConverter<ProjectLinkDto, ProjectLink> DtoEntityConverter;

        public ProjectLinkDto()
        {
            message = "";
            DtoEntityConverter = new DtoEntityConverter<ProjectLinkDto, ProjectLink>();
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
        public ProjectLinkDto MakeThisObject()
        {
            ProjectLinkDto thisObject = new ProjectLinkDto()
            {
                ID = ID,
                Link = Link,
                ProjectID = ProjectID,
                Service = Service


            };

            return thisObject;

        }
        private void SetThisObject(ProjectLinkDto dto)
        {

            ID = dto.ID;
            Link = dto.Link;
            ProjectID = dto.ProjectID;
            Service = dto.Service;
        }

        public ProjectLink ConvertToEntity()
        {
            ProjectLink ConvertedTo = DtoEntityConverter.ConvertToCoreEntity(MakeThisObject());
            return ConvertedTo;
        }

        public ProjectLinkDto ConvertToDto(ProjectLink CoreEntity)
        {
            ProjectLinkDto ConvertedTo = DtoEntityConverter.ConvertToDto(CoreEntity);
            SetThisObject(ConvertedTo);
            return ConvertedTo;
        }
    }
}