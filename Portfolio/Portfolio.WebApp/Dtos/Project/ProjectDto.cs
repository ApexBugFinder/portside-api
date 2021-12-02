using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Portfolio.WebApp.Abstract;
using Portfolio.WebApp.Controllers;
using Portfolio.WebApp.Helpers;
using Portfolio.WebApp.Services;
using Portfolio.PorfolioDomain.Core.Entities;

namespace Portfolio.WebApp.Dtos
{
    public class ProjectDto
    {
        private string message;
        public string ID { get; set; }

        public string ProjectName { get; set; }

        public string ProjectCreatorID { get; set; }

        public string Description { get; set; }

        public DateTime Started { get; set; }

        public DateTime Completed { get; set; }

        public string Banner { get; set; }

        public string SmallBanner { get; set; }

        public Boolean Published { get; set; }

        public List<ProjectRequirementDto> ProjectRequirements { get; set; }

        public List<ProjectLinkDto> ProjectLinks { get; set; }

        public List<PublishedHistoryDto> ProjectHistory { get; set; }


        private ProjectRequirementsController prController { get; set; }

        private ProjectLinksController plController { get; set; }

        private PortfolioContext PortfolioContext;

        private DtoEntityConverter<ProjectDto, Project> DtoEntityConverter;
        public ProjectDto()
        {
            message = "";
            PortfolioContext = PortfolioContext.Instance;
            this.ProjectRequirements =  new List<ProjectRequirementDto>();
            this.ProjectLinks = new List<ProjectLinkDto>();
            this.ProjectHistory =  new List<PublishedHistoryDto>();
            this.DtoEntityConverter = new DtoEntityConverter<ProjectDto, Project>();

        }
        public void AddContext(IProjectRequirementRepository repo, IProjectLinkRepository linkRepo)
        {
            prController = new ProjectRequirementsController(repo);
            plController = new ProjectLinksController(linkRepo);
        }
        public void Print()
        {
            message = "Project Object Print Report"
                + "\nProject ID: " + this.ID
                + "\nProject ProjectId: " + this.ProjectName
                + "\nProject Description: " + this.Description
                + "\nProject Banner Url: " + this.Banner
                + "\nProject Small Banner Url: " + this.SmallBanner

                + "\nProject Started: " + this.Started
                + "\nProject Completed: " + this.Completed
                + "\nProject IsPublished: " + this.Published
                ;


            Notification.PostMessage(message);




        }
        public ProjectDto MakeThisObject()
        {

            ProjectDto thisObject = new ProjectDto()
            {
                ID = ID,
                ProjectName = ProjectName,
                ProjectCreatorID = ProjectCreatorID,
                Description = Description,
                Started = Started,
                Completed = Completed,
                Banner = Banner,
                SmallBanner = SmallBanner,
                Published = Published,
                ProjectRequirements = ProjectRequirements,
                ProjectLinks = ProjectLinks,
                ProjectHistory = ProjectHistory

            };

            return thisObject;

        }
        private void SetThisObject(ProjectDto dto)
        {

            ID = dto.ID;
            ProjectName = dto.ProjectName;
            ProjectCreatorID = dto.ProjectCreatorID;
            Description = dto.Description;
            Started = dto.Started;
            Completed = dto.Completed;
            Banner = dto.Banner;
            SmallBanner = dto.SmallBanner;
            Published = dto.Published;
            ProjectRequirements = dto.ProjectRequirements;
            ProjectLinks = dto.ProjectLinks;
            ProjectHistory = dto.ProjectHistory;


        }

        public async Task<List<ProjectRequirementDto>> UpdateProjectRequirements()
        {

            List<ProjectRequirementDto> updated = await prController.PutProjectRequirementRangeInternal(ProjectRequirements);
            ProjectRequirements = updated;
            return ProjectRequirements;



        }

        public async Task<List<ProjectLinkDto>> UpdateProjectLinks()
        {

            List<ProjectLinkDto> updated = await plController.PutProjectLinkRangeInternal(this.ProjectLinks);
            return updated;
        }

        public Project CovertToCoreEntity()
        {
            Project ConvertedTo = DtoEntityConverter.ConvertToCoreEntity(MakeThisObject());
            return ConvertedTo;
        }

        public ProjectDto ConvertToDto(Project CoreEntity)
        {
            ProjectDto ConvertedTo = DtoEntityConverter.ConvertToDto(CoreEntity);
            SetThisObject(ConvertedTo);
            return ConvertedTo;
        }
    }
}
