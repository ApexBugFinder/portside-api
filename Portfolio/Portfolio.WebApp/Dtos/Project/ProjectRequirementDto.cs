using AutoMapper;
using Portfolio.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.WebApp.Controllers;
using Portfolio.WebApp.Services;
using Portfolio.WebApp.Concrete;
using Portfolio.WebApp.Abstract;

namespace Portfolio.WebApp.Dtos
{
    public class ProjectRequirementDto
    {
        private string message;

        public string ID { get; set; }

        public string ProjectID { get; set; }

        public string EditState { get;  set; }

        // public virtual ProjectDto Project { get; set; }

        public string Requirement { get; set; }

        private ProjectRequirementsController prController { get; set; }

        private PortfolioContext PortfolioContext;

        public ProjectRequirementDto()
        {
            message = "";
            PortfolioContext = PortfolioContext.Instance;

        }

        public void Print()
        {
            message = "ProjectRequirements Object Print Report"
                + "\nProjectRequirement ID: " + this.ID
                + "\nProjectRequirement ProjectId: " + this.ProjectID
                + "\nProjectRequirements Requirement: " + this.Requirement
                + "\nProjectRequirements EditState: " + this.EditState
                ;


            Notification.PostMessage(message);




        }
        public void AddContext(IProjectRequirementRepository repo)
        {
           prController = new ProjectRequirementsController(repo);

        }

        private ProjectRequirementDto MakeThisObject()
        {
            ProjectRequirementDto thisObject = new ProjectRequirementDto()
            {
                ID = ID,
                ProjectID = ProjectID,
                EditState = EditState,
                Requirement = Requirement

            };
            return thisObject;
        }
        // public async Task<ProjectRequirementDto> DeleteRequirement()
        // {
        //     var reqToDelete = MakeThisObject();
            // prController = new ProjectRequirementsController(new EFProjectRequirementRepository(PortfolioContext));
            // var reqExists = await prController.GetRequirementByIdInternal(reqToDelete);
            // if(!string.IsNullOrEmpty(reqExists.ID))
            // {
            //   reqToDelete =  await prController.DeleteProjectRequirementInternal(reqExists.ID);
            //     Console.WriteLine("Requirment Deleted: ", reqToDelete.ID);

            // }
        //     return reqToDelete;
        // }



    }
}
