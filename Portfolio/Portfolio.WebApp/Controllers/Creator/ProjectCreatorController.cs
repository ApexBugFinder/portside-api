
using Microsoft.AspNetCore.Mvc;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Abstract;
using Portfolio.WebApp.Dtos;
using Portfolio.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.WebApp.Concrete;
using Portfolio.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Cors;

namespace Portfolio.WebApp.Controllers
{
  // [EnableCors("MyAllowSpecificOrigins")]
  [Route("api/[controller]")]
  [ApiController]
  // [ControllerName("pc")]
  //[ValidateAntiForgeryToken]
//   [Authorize]
  public class ProjectCreatorController : ControllerBase
  {
    private readonly IProjectCreatorRepository repository;
    private string message = "";
    private DtoEntityConverter<ProjectCreatorDto, ProjectCreator> entityConverter;
    private DataConverter<ProjectCreatorDto> dataConverter;

    public ProjectCreatorController(IProjectCreatorRepository projectCreatorRepository)
    {
      repository = projectCreatorRepository;
      entityConverter = new DtoEntityConverter<ProjectCreatorDto, ProjectCreator>();
      dataConverter = new DataConverter<ProjectCreatorDto>();
    }


    // [DisableCors()]
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<ProjectCreatorDto>>> GetProjectCreators()
    {
      List<ProjectCreatorDto> creatorsDtosFound = new List<ProjectCreatorDto>();
      try
      {
        List<ProjectCreator> projectCreators = await repository.GetItems();
        projectCreators.ForEach(creator => creatorsDtosFound.Add(entityConverter.ConvertToDto(creator)));

        return creatorsDtosFound;
      }
      catch (Exception ex)
      {
        message = "***ERROR IN CONTROLLER" +
            "\nIN CLASS: ProjectCreator" +
            "\nWITH METHOD: GetProjectCreators" +
            "\nERROR MESSAGE: " + ex.Message;
        Notification.PostMessage(message);
        return BadRequest(message);
      }
    }

    [AllowAnonymous]
    [DisableCors()]
    // Get Project Creator: just a basic Read of DB value
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectCreatorDto>> GetProjectCreator(string id)
    {
      ProjectCreatorDto projectCreatorFoundDto = new ProjectCreatorDto();
      if (await this.ProjectCreatorExists(id))
      {
        try
        {
          ProjectCreator projectCreatorFound = await repository.Read(id);
          if (projectCreatorFound != null)
          {
            projectCreatorFoundDto = entityConverter.ConvertToDto(projectCreatorFound);

            return Ok(projectCreatorFoundDto);
          }
          else
          {
            return NotFound("No ProjectCreators found");
          }
        }
        catch (Exception ex)
        {
          message = "***ERROR IN CONTROLLER" +
          "\nIN CLASS: ProjectCreator" +
          "\nWITH METHOD: GetProjectCreator" +
          "\nERROR MESSAGE: " + ex.Message;
          Notification.PostMessage(message);
          return BadRequest(message);
        }
      }
      return NotFound("ProjectCreator Does Not Exist");
    }

    public async Task<ProjectCreatorDto> GetProjectCreatorInfoInternal(string id)
    {
      var result = await GetProjectCreatorInfo(id);
      DataConverter<ProjectCreatorDto> d = new DataConverter<ProjectCreatorDto>();
      ProjectCreatorDto returnResult = d.GetOkObjectResult(result);
      return returnResult;
    }
    // GET: api/ProjectCreator/5
    // [AllowAnonymous]
    // Gets the User info By UserID: All user Projects Experiences & Education
    [HttpGet("userID/{id}")]
    // [DisableCors()]
    public async Task<ActionResult<ProjectCreatorDto>> GetProjectCreatorInfo(string id)
    {
      ProjectCreatorDto projectCreatorFoundDto = new ProjectCreatorDto();
      if (await this.ProjectCreatorExists(id))
      {
        try
        {
          ProjectCreator projectCreatorFound = await repository.GetItemByPC(id);
          // Debug.WriteLine(projectCreatorFound);
          if (projectCreatorFound != null)
          {
            projectCreatorFoundDto = entityConverter.ConvertToDto(projectCreatorFound);

            return Ok(projectCreatorFoundDto);
          }

        }
        catch (Exception ex)
        {
          message = "***ERROR IN CONTROLLER" +
       "\nIN CLASS: ProjectCreator" +
       "\nWITH METHOD: GetProjectCreatorInfo" +
       "\nERROR MESSAGE: " + ex.Message;
          Notification.PostMessage(message);
          return BadRequest(message);
        }
      }
      else
      {
        ProjectCreatorDto newUser = new ProjectCreatorDto();

        newUser.ID = id;
        newUser.Username = "portfolioUser_" + id;
        return Ok(await PostProjectCreator(newUser));
        //
      }
      return NotFound("No ProjectCreators found");
    }

    // GETS all user info by username: Projects, Experiences & Education

    public async Task<ProjectCreatorDto> GetProjectCreatorInfoByUsernameInternal(string id)
    {
      var result = await GetProjectCreatorInfoByUsername(id);
      DataConverter<ProjectCreatorDto> d = new DataConverter<ProjectCreatorDto>();
      ProjectCreatorDto returnResult = d.GetOkObjectResult(result);
      return returnResult;
    }
    // GET: api/ProjectCreator/5
    // [DisableCors()]
    [AllowAnonymous]
    [HttpGet("username/{uname}")]
    public async Task<ActionResult<ProjectCreatorDto>> GetProjectCreatorInfoByUsername(string uname)
    {
      ProjectCreatorDto projectCreatorFoundDto = new ProjectCreatorDto();


      try
      {
        ProjectCreator projectCreatorFound = await repository.GetItemByUserName(uname);
        if (projectCreatorFound != null)
        {
          projectCreatorFoundDto = entityConverter.ConvertToDto(projectCreatorFound);

          return Ok(projectCreatorFoundDto);
        }
        else
        {
          return NotFound("No ProjectCreators found");
        }
      }
      catch (Exception ex)
      {
        message = "***ERROR IN CONTROLLER" +
           "\nIN CLASS: ProjectCreator" +
           "\nWITH METHOD: GetProjectCreator" +
           "\nERROR MESSAGE: " + ex.Message;
        Notification.PostMessage(message);
        return BadRequest(message);
      }

      //   return NotFound("ProjectCreator Does Not Exist");

    }

    // searches all userNAMES AND RETURNS USER: Projects, Experiences & Education
    //public async Task<ProjectCreatorDto> SearchByUsernameInternal(string id)
    //{
    //    var result = await SearchUsernameByKeyword(id);
    //    DataConverter<ProjectCreatorDto> d = new DataConverter<ProjectCreatorDto>();
    //    ProjectCreatorDto returnResult = d.GetOkObjectResult(result);
    //    return returnResult;
    //}
    // GET: api/ProjectCreator/5
    // [DisableCors()]
    [AllowAnonymous]
    [HttpPut("search/")]

    public async Task<ActionResult<ProjectCreatorDto>> SearchUsernameByKeyword([FromBody] string keyword)
    {
      List<ProjectCreatorDto> projectCreatorFoundDto = new List<ProjectCreatorDto>();

      Notification.PostMessage(keyword);
      try
      {
        List<ProjectCreator> projectCreatorsFound = await repository.SearchForPCsByKeyword(keyword);
        projectCreatorsFound.ForEach( i => {
          Notification.PostMessage(i.SubjectId);
          Notification.PostMessage(i.Username);

        });
        if (projectCreatorsFound != null && projectCreatorsFound.Count > 0)
        {
          projectCreatorsFound.ForEach(i => projectCreatorFoundDto.Add(entityConverter.ConvertToDto(i)));

          return Ok(projectCreatorFoundDto);
        }
        else
        {
          return NotFound("No ProjectCreators found");
        }
      }
      catch (Exception ex)
      {
        message = "***ERROR IN CONTROLLER" +
           "\nIN CLASS: ProjectCreator" +
           "\nWITH METHOD: SEARCH FOR ProjectCreators By Keyword" +
           "\nERROR MESSAGE: " + ex.Message;
        Notification.PostMessage(message);
        return BadRequest(message);
      }

      //return NotFound("ProjectCreator Does Not Exist");

    }



    // PUT: api/ProjectCreator/5
    [HttpPut("update/{id}")]
    public async Task<ActionResult<ProjectCreatorDto>> PutProjectCreator(string id, [FromBody] ProjectCreatorDto projectCreatorDto)
    {
      if (id != projectCreatorDto.ID)
      {
        return BadRequest("Bad Request, please try again");
      }

      ProjectCreator projectCreatorUpdated = new ProjectCreator();
      ProjectCreatorDto updatedProjectCreatorDto = new ProjectCreatorDto();

      if (await this.ProjectCreatorExists(id))
      {
        try
        {
          ProjectCreator projectCreatorToUpdate = entityConverter.ConvertToCoreEntity(projectCreatorDto);
          projectCreatorUpdated = await repository.Update(projectCreatorToUpdate);
          updatedProjectCreatorDto = entityConverter.ConvertToDto(projectCreatorUpdated);

          return Ok(updatedProjectCreatorDto);
        }

        catch (Exception ex)
        {
          message = "***ERROR IN CONTROLLER" +
             "\nCLASS: ProjectCreator" +
             "\nWITH METHOD: PutProjectCreator()" +
             "\nERROR MESSAGE: " + ex.Message;
          Notification.PostMessage(message);
          return BadRequest(message);
        }



      }
      return NotFound("ProjectCreator Does not Exist");


    }
    public async Task<ProjectCreatorDto> PostProjectCreatorInternal(ProjectCreatorDto projectCreatorDto)
    {
      var result = await PostProjectCreator(projectCreatorDto);

      ProjectCreatorDto returnResult = dataConverter.GetOkObjectResult(result);


      return returnResult;
    }
    // POST: api/Allergies
    [HttpPost]
    public async Task<ActionResult<ProjectCreatorDto>> PostProjectCreator([FromBody] ProjectCreatorDto projectCreatorDto)
    {

        message = "ID of user to create: " + projectCreatorDto.ID;
        Notification.PostMessage(message);
      ProjectCreator projectCreatorToCreate = entityConverter.ConvertToCoreEntity(projectCreatorDto);


      if (projectCreatorDto.ID != "" && await this.ProjectCreatorExists(projectCreatorDto.ID))
      {
        return Conflict("ProjectCreator already exists");
      }
      try
      {
        ProjectCreator projectCreatorCreated = await repository.Create(projectCreatorToCreate);
        ProjectCreatorDto projectCreatorCreatedDto = entityConverter.ConvertToDto(projectCreatorCreated);

        return Ok(projectCreatorCreatedDto);
      }

      catch (Exception ex)
      {
        message = "***ERROR IN CONTROLLER" +
           "\nIN CLASS: ProjectCreator" +
               "\nWITH METHOD: PostProjectCreator" +
           "\nERROR MESSAGE: " + ex.Message;
        Notification.PostMessage(message);
        return BadRequest(message);
      }


    }



    public async Task<ProjectCreatorDto> DeleteProjectCreatorInternal(string id)
    {
      var result = await DeleteProjectCreator(id);
      DataConverter<ProjectCreatorDto> d = new DataConverter<ProjectCreatorDto>();
      ProjectCreatorDto returnResult = d.GetOkObjectResult(result);


      return returnResult; ;
    }
    // DELETE: api/Allergies/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<ProjectCreatorDto>> DeleteProjectCreator(string id)
    {
      if (id != string.Empty && await this.ProjectCreatorExists(id))
      {
        try
        {
          ProjectCreatorDto projectCreatorDeleted = new ProjectCreatorDto();
          ProjectCreator projectCreator = await repository.Read(id);

          await repository.Delete(projectCreator.SubjectId);
          return Ok(entityConverter.ConvertToDto(projectCreator));


        }

        catch (Exception ex)
        {
          message = "***ERROR PROJECTCREATOR CONTROLLER" +
             "\nIN CLASS: ProjectCreator" +
             "\nWITH METHOD: DeleteProjectCreator" +
             "\nERROR MESSAGE: " + ex.Message;
          Notification.PostMessage(message);
          return BadRequest(message);
        }
      }
      return NotFound("Cannot find ProjectCreator to delete");
    }



    private async Task<bool> ProjectCreatorExists(string id)
    {
      return await repository.Exists(id);
    }

// static async Task<TokenResponse> RequestTokenAsync()
// {
//     var handler = new SocketsHttpHandler();
//     var cert = new X509Certificate2("client.p12", "password");
//     handler.SslOptions.ClientCertificates = new X509CertificateCollection { cert };

//     var client = new HttpClient(handler);

//     var disco = await client.GetDiscoveryDocumentAsync(Constants.Authority);
//     if (disco.IsError) throw new Exception(disco.Error);

//     var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
//     {
//         Address = disco
//                         .TryGetValue(OidcConstants.Discovery.MtlsEndpointAliases)
//                         .Value<string>(OidcConstants.Discovery.TokenEndpoint)
//                         .ToString(),

//         ClientId = "mtls",
//         Scope = "api1"
//     });

//     if (response.IsError) throw new Exception(response.Error);
//     return response;
// }



  }
}
