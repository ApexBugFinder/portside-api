using Portfolio.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.WebApp.Services;
using Portfolio.WebApp.Abstract;
using Portfolio.WebApp.Controllers;
using Portfolio.PorfolioDomain.Core.Entities;
namespace Portfolio.WebApp.Dtos

{
    public class ExperienceDto
    {
        private string message = "";


        public string ID { get; set; }


        public string ProjectCreatorID { get; set; }



        public string Company { get; set; }
        public string Title { get; set; }
        public string LogoUrl { get; set; }
        public DateTime Started { get; set; }
        public DateTime Completed { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public List<RoleDto> Roles { get; set; }

        private PortfolioContext PortfolioContext;
        //private ExperiencesController experiencesController;
        private RolesController rolesController;
        private ExperiencesController experienceController;
        private DtoEntityConverter<ExperienceDto, Experience> DtoentityConverter;

        public ExperienceDto()
        {

            message = "";
            PortfolioContext = PortfolioContext.Instance;
            DtoentityConverter = new DtoEntityConverter<ExperienceDto, Experience>();
            Roles = new List<RoleDto>();
        }

        public void AddContext(IRolesRepository repo)
        {
            rolesController = new RolesController(repo);
        }

        public void Print()
        {
            message = "ExperienceDto Object Print Report"
                + "\nExperienceDto ID: " + this.ID
                + "\nExperienceDto ProjectCreatorId: " + this.ProjectCreatorID
                + "\nExperienceDto has " + Roles.Count + " Roles and then are: "

                ;
            this.Roles.ForEach(i =>
            {
                    message += message + i.ID + ": " + i.MyRole;
            });


            Notification.PostMessage(message);
            for (int i = 0; i <= Roles.Count; i++)
            {
                Roles[i].Print();
            }
        }

        private ExperienceDto MakeThisObject()
        {
            ExperienceDto thisObject = new ExperienceDto()
            {
                ID = ID,
                ProjectCreatorID = ProjectCreatorID,

                Company = Company,
                Title = Title,
                LogoUrl = LogoUrl,
                Started = Started,
                Completed = Completed,
                City = City,
                State = State,
                Roles = Roles
            };
            return thisObject;
        }


        public void SetThisObject(ExperienceDto dto)
        {
                ID = dto.ID;
                ProjectCreatorID = dto.ProjectCreatorID;

                Company = dto.Company;
                Title = dto.Title;
                LogoUrl = dto.LogoUrl;
                Started = dto.Started;
                Completed = dto.Completed;
                City = dto.City;
                State = dto.State;
                Roles = dto.Roles;
        }

        public Experience ConvertToEntity()
        {
            Experience ConvertedToEntity = DtoentityConverter.ConvertToCoreEntity(MakeThisObject());
            return ConvertedToEntity;
        }

        public ExperienceDto ConvertToDto(Experience entity)
        {
            ExperienceDto ConvertedToDto = DtoentityConverter.ConvertToDto(entity);
            SetThisObject(ConvertedToDto);
            return ConvertedToDto;
        }

        public async Task<List<RoleDto>> UpdateRoles()
        {

            List<RoleDto> updated = await rolesController.PutRoleRangeInternal(Roles);
            Roles = updated;
            return updated;
        }


        public async Task<ExperienceDto> GetThisExperience(IExperienceRepository repo)
        {
            this.experienceController = new ExperiencesController(repo);
            SetThisObject(await this.experienceController.GetExperienceByIdInternal(ID));
            return MakeThisObject();
        }

        public async Task<List<RoleDto>> GetRoles()
        {
            this.Roles.AddRange(await rolesController.GetRolesByExperienceIdInternal(ID));

            return this.Roles;
        }

    }
}
