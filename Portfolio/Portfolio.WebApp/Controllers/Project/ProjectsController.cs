
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Dtos;
using Portfolio.WebApp.Helpers;
using Portfolio.WebApp.Abstract;
using Portfolio.WebApp.Services;
using Portfolio.WebApp.Concrete;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;


namespace Portfolio.WebApp.Controllers
{
  [EnableCors("MyAllowSpecificOrigins")]
  [Route("api/[controller]")]
  [ApiController]
  public class ProjectsController : ControllerBase
  {
    private readonly IProjectRepository repository;
    private readonly IProjectRequirementRepository repoReq;
    private readonly IProjectLinkRepository repoLink;
    private string message = "";
    DtoEntityConverter<ProjectDto, Project> entityConverter;
    DataConverter<ProjectDto> dataConverter;
    private readonly PortfolioContext context;
    public ProjectsController(IProjectRepository Repository, IProjectRequirementRepository requirementRepository, IProjectLinkRepository linkRepo)
    {
      repository = Repository;
      entityConverter = new DtoEntityConverter<ProjectDto, Project>();
      dataConverter = new DataConverter<ProjectDto>();
      context = PortfolioContext.Instance;
      repoLink = linkRepo;
      repoReq = requirementRepository;


    }




    // GET: api/Project
     [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetProjects()
    {
      List<ProjectDto> projectsDtosFound = new List<ProjectDto>();
      try
      {
        List<Project> projects = await repository.GetItems();
        projects.ForEach(creator =>
        {

          projectsDtosFound.Add(entityConverter.ConvertToDto(creator));

        });
        return projectsDtosFound;
      }

      catch (Exception ex)
      {
        message = "***ERROR IN CONTROLLER" +
          "\nIN CLASS: Project" +
               "\nWITH METHOD: GetProjects" +
           "\nERROR MESSAGE: " + ex.Message;
        Notification.PostMessage(message);
        return BadRequest(message);
      }

    }
    // GET: api/Projects/{id}
     [AllowAnonymous]
    [HttpGet("all/{projectCreatorID}")]
    public  async Task<ActionResult<List<ProjectDto>>> GetProjectsById(string projectCreatorID)
    {
      List<ProjectDto> projectsDtosFound = new List<ProjectDto>();
      try
      {
        List<Project> projects = await repository.GetItemsByPCAsync(projectCreatorID).ConfigureAwait(false);

        foreach(var item in projects) {
          projectsDtosFound.Add(entityConverter.ConvertToDto(item));
        }
        return projectsDtosFound.ToList();
      }
      catch (Exception ex)
      {
        message = "***ERROR IN CONTROLLER" +
          "\nIN CLASS: Project" +
               "\nWITH METHOD: GetProjects" +
           "\nERROR MESSAGE: " + ex.Message;
        Notification.PostMessage(message);
        return BadRequest(message);
      }

    }


    public async Task<ProjectDto> GetProjectInternal(string id)
    {
      var result = await GetProject(id);
      DataConverter<ProjectDto> d = new DataConverter<ProjectDto>();
      ProjectDto returnResult = d.GetOkObjectResult(result);


      return returnResult;
    }


    // GET: api/Project/5
    [AllowAnonymous]
    [HttpGet("project/{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(string id)
    {
      ProjectDto projectFoundDto = new ProjectDto();
      if (await this.ProjectExists(id))
      {
        try
        {
          Project projectFound = await repository.Read(id);
          if (projectFound != null)
          {
            projectFoundDto = entityConverter.ConvertToDto(projectFound);

            return Ok(projectFoundDto);
          }
          else
          {
            return NotFound("No Projects found");
          }

        }

        catch (Exception ex)
        {
          message = "***ERROR IN CONTROLLER" +
             "\nIN CLASS: Project" +
             "\nWITH METHOD: GetProject" +
             "\nERROR MESSAGE: " + ex.Message;
          Notification.PostMessage(message);
          return BadRequest(message);
        }


      }
      return NotFound("Project Does Not Exist");

    }



    public async Task<ProjectDto> PutProjectInternal(ProjectDto projectDto)
    {
      var result = await PutProject(projectDto.ID, projectDto);

      ProjectDto returnResult = dataConverter.GetOkObjectResult(result);


      return returnResult;
    }
    // PUT: api/Project/5
    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ActionResult<ProjectDto>> PutProject(string id, [FromBody] ProjectDto projectDto)
    {



      // if (id != projectDto.ID || string.IsNullOrEmpty(projectDto.ProjectCreatorID))
      // {
      //   this.message = "id: " + Newtonsoft.Json.JsonConvert.SerializeObject(id);
      // this.message += "\n Project to be posted: " + Newtonsoft.Json.JsonConvert.SerializeObject(projectDto.ToString());
      // Notification.PostMessage(this.message);
      //   return BadRequest("Bad Request, please try again");
      // }
      // this.message =  "ProjectCreator ID: " + Newtonsoft.Json.JsonConvert.SerializeObject(projectDto.ProjectCreatorID);
      // this.message += this.message + "\nProject to be posted:"  + projectDto;
      // Notification.PostMessage(this.message);
      Project projectUpdated = new Project();

      //

      // if (await this.ProjectExists(id))
      // {
        try
        {

                    // projectDto.AddContext(new EFProjectRequirementRepository(context), new EFProjectLinkRepository(context));


                    // await projectDto.UpdateProjectRequirements();
                    // //projectDto.ProjectRequirements = new List<ProjectRequirementDto>();
                    // var reqsRemain = projectDto.ProjectRequirements.Count;


                    // // Project Links
                    // projectDto.ProjectLinks = await projectDto.UpdateProjectLinks();

                    Project projectToUpdate = entityConverter.ConvertToCoreEntity(projectDto);

                    projectUpdated = await repository.Update(projectToUpdate);
                   ProjectDto updatedProjectDto = entityConverter.ConvertToDto(projectUpdated);
                    this.message += this.message + "\nProject UPDATED:"  + JsonConvert.SerializeObject(projectDto.ToString());
                    Notification.PostMessage(this.message);
                    // updatedProjectDto = entityConverter.ConvertToDto(projectUpdated);
                    // //updatedProjectDto.ProjectRequirements = reqsUpdated;
          // updatedProjectDto.ProjectRequirements.AddRange(reqsUpdated);
          return Ok(updatedProjectDto);
        }

        catch (Exception ex)
        {
          message = "***ERROR IN CONTROLLER" +
             "\nCLASS: Project" +
             "\nWITH METHOD: PutProject()" +
             "\nERROR MESSAGE: " + ex.Message;
          Notification.PostMessage(message);
          return BadRequest(message);
        }



      // }

      // return NotFound("Project Does not Exist");


    }

    public async Task<ProjectDto> PostProjectInternal(ProjectDto projectDto)
    {
      var result = await PostProject(projectDto);

      ProjectDto returnResult = dataConverter.GetOkObjectResult(result);


      return returnResult;
    }
    // POST: api/Projects/new
    [HttpPost("new")]
    [AllowAnonymous]
    public async Task<ActionResult<ProjectDto>> PostProject([FromBody] ProjectDto projectDto)
    {
            // var filteredReqs = projectDto.ProjectRequirements.Where(i => i.EditState == "add");
            // projectDto.ProjectRequirements = new List<ProjectRequirementDto>() {

            // };
            // projectDto.ProjectRequirements.AddRange(filteredReqs);
            // await projectDto.UpdateProjectRequirements();
             Project projectToCreate = entityConverter.ConvertToCoreEntity(projectDto);

      if (await this.ProjectExists(projectDto.ID))
      {
        return Conflict("Project already exists or does not have a valid ID");
      }
      try
      {

        this.message = "\nProject TO BE POSTED:" + JsonConvert.SerializeObject(projectDto.ToString());
        Notification.PostMessage(this.message);

        Project projectCreated = await repository.Create(projectToCreate);

        ProjectDto projectCreatedDto = entityConverter.ConvertToDto(projectCreated);
        this.message = "\nProject POSTED:" + JsonConvert.SerializeObject(projectCreatedDto.ToString());
        Notification.PostMessage(this.message);
        return Ok(projectCreatedDto);
      }

      catch (Exception ex)
      {
        message = "***ERROR IN CONTROLLER" +
           "\nIN CLASS: Project" +
               "\nWITH METHOD: PostProject" +
           "\nERROR MESSAGE: " + ex.Message;
        Notification.PostMessage(message);
        return BadRequest(message);
      }


    }



    public async Task<ProjectDto> DeleteProjectInternal(string id)
    {
      var result = await DeleteProject(id);

      ProjectDto returnResult = dataConverter.GetOkObjectResult(result);


      return returnResult; ;
    }
    // DELETE: api/Projects/5
    [HttpDelete("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProjectDto>> DeleteProject(string id)
    {
      if (id != string.Empty && await this.ProjectExists(id))
      {
        try
        {

          Project project = await repository.Read(id);

          await repository.Delete(id);
          return Ok(entityConverter.ConvertToDto(project));


        }

        catch (Exception ex)
        {
          message = "***ERROR PROJECT CONTROLLER" +
             "\nIN CLASS: Project" +
             "\nWITH METHOD: DeleteProject" +
             "\nERROR MESSAGE: " + ex.Message;
          Notification.PostMessage(message);
          return BadRequest(message);
        }
      }
      return NotFound("Cannot find Project to delete");
    }



    private async Task<bool> ProjectExists(string id)
    {
      return await repository.Exists(id);
    }

  }
}
