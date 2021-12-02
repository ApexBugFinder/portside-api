
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
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Portfolio.WebApp.Controllers
{
  [EnableCors("MyAllowSpecificOrigins")]
  [Route("api/[controller]")]
  [ApiController]

  public class ExperiencesController : ControllerBase
  {
    private readonly IExperienceRepository repository;
    private string message = "";
    DtoEntityConverter<ExperienceDto, Experience> entityConverter;
    DataConverter<ExperienceDto> dataConverter;
    private readonly PortfolioContext context;

    public ExperiencesController(IExperienceRepository Repository)
    {
      repository = Repository;
      entityConverter = new DtoEntityConverter<ExperienceDto, Experience>();
      dataConverter = new DataConverter<ExperienceDto>();
      context = PortfolioContext.Instance;
    }

    // GET: api/<ExperiencesController>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<ExperienceDto>>> GetExperiences()
    {
      List<ExperienceDto> itemDtosFound = new List<ExperienceDto>();
      try
      {
        List<Experience> itemsFound = await repository.GetItems();
        itemsFound.ForEach(item =>
        {

          itemDtosFound.Add(entityConverter.ConvertToDto(item));

        });
        return Ok(itemDtosFound);
      }

      catch (Exception ex)
      {
        message = "***ERROR IN CONTROLLER" +
          "\nIN CLASS: ExperienceController" +
               "\nWITH METHOD: GetExperiences" +
           "\nERROR MESSAGE: " + ex.Message;
        Notification.PostMessage(message);
        return BadRequest(message);
      }

    }


    public async Task<ExperienceDto> GetExperienceByIdInternal(string itemId)
    {
      var result = await GetExperienceByID(itemId);

      ExperienceDto returnResult = dataConverter.GetOkObjectResult(result);


      return returnResult;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ExperienceDto>> GetExperienceByID(string id)
    {

      Experience itemFound = new Experience();
      ExperienceDto itemFoundDto = new ExperienceDto();
      if (!(await ItemExists(id)))
      {
        message = "Item Does not Exist";
        Notification.PostMessage(message);
        return BadRequest(message);
      }
      try
      {
        itemFound = await repository.Read(id);

      }
      catch (Exception ex)
      {
        message = "***ERROR IN EXPERIENCE CONTROLLER" +
         "\nIN CLASS: ExperienceController" +
              "\nWITH METHOD: GetExperienceByID" +
          "\nERROR MESSAGE: " + ex.Message;
        Notification.PostMessage(message);
        return BadRequest(message);

      }

      return itemFoundDto.ConvertToDto(itemFound);

    }
    // GET api/<ExperiencesController>/5
    [HttpGet("all/{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<ExperienceDto>>> GetExperienceByProjectCreatorId(string id)
    {
      List<ExperienceDto> itemsDtosFound = new List<ExperienceDto>();
      try
      {
        List<Experience> itemsFound = await repository.GetItemsByPC(id);

        itemsFound.ForEach(async project =>
        {
          ExperienceDto itemFoundDto = new ExperienceDto();
          itemsFound.ForEach(i =>
                  {
              itemsDtosFound.Add(itemFoundDto.ConvertToDto(project));
            });



        });
        return itemsDtosFound;
      }

      catch (Exception ex)
      {
        message = "***ERROR IN EXPERIENCE CONTROLLER" +
          "\nIN CLASS: ExperienceController" +
               "\nWITH METHOD: GetExperienceByProjectCreatorId" +
           "\nERROR MESSAGE: " + ex.Message;
        Notification.PostMessage(message);
        return BadRequest(message);
      }

    }

    public async Task<ExperienceDto> PutExperienceInternal(ExperienceDto itemToUPdatetDto)
    {
      var result = await PutExperience(itemToUPdatetDto.ID, itemToUPdatetDto);

      ExperienceDto returnResult = dataConverter.GetOkObjectResult(result);


      return returnResult;
    }
    // PUT: api/Experiences/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ExperienceDto>> PutExperience(string id, [FromBody] ExperienceDto itemToUpdateDto)
    {
      if (id != itemToUpdateDto.ID)
      {
        return BadRequest("Bad Request, please try again");
      }

      Experience itemUpdated = new Experience();
      ExperienceDto updateditemDto = new ExperienceDto();

      if (await this.ItemExists(id))
      {
        try
        {





          Experience itemToUpdate = entityConverter.ConvertToCoreEntity(itemToUpdateDto);
          itemUpdated = await repository.Update(itemToUpdate);
          updateditemDto = entityConverter.ConvertToDto(itemUpdated);

          return Ok(updateditemDto);
        }

        catch (Exception ex)
        {
          message = "***ERROR IN PROJECT_REQUIREMENT CONTROLLER" +
             "\nCLASS: Experience" +
             "\nWITH METHOD: PutExperience()" +
             "\nERROR MESSAGE: " + ex.Message;
          Notification.PostMessage(message);
          return BadRequest(message);
        }



      }
      return NotFound("Experience Does not Exist");


    }
    public async Task<ExperienceDto> PostExperienceInternal(ExperienceDto itemDto)
    {
      var result = await PostExperience(itemDto);

      ExperienceDto returnResult = dataConverter.GetOkObjectResult(result);


      return returnResult;
    }
    // POST: api/Experiences
    [HttpPost("new")]
    public async Task<ActionResult<ExperienceDto>> PostExperience([FromBody] ExperienceDto itemToPostDto)
    {


      List<RoleDto> rolesToAdd = itemToPostDto.Roles.Where(i => i.EditState == "add").ToList();
      itemToPostDto.Roles = new List<RoleDto>();

      Experience itemToCreate = entityConverter.ConvertToCoreEntity(itemToPostDto);
      itemToCreate.ID = Guid.NewGuid().ToString();

      if (itemToPostDto.ID != "" && await this.ItemExists(itemToPostDto.ID))
      {
        return Conflict("Project already exists");
      }
      try
      {
        Experience itemCreated = await repository.Create(itemToCreate);
        ExperienceDto itemCreatedDto = entityConverter.ConvertToDto(itemCreated);
        if (rolesToAdd.Count > 0)
        {
          itemCreatedDto.Roles = rolesToAdd;
          itemCreatedDto.AddContext(new EFRoleRepository(context));
          await itemCreatedDto.UpdateRoles();
        }
        return Ok(itemCreatedDto);
      }

      catch (Exception ex)
      {
        message = "***ERROR IN EXPERIENCES CONTROLLER" +
           "\nIN CLASS: Experience" +
               "\nWITH METHOD: PostExperience" +
           "\nERROR MESSAGE: " + ex.Message;
        Notification.PostMessage(message);
        return BadRequest(message);
      }


    }



    public async Task<ExperienceDto> DeleteExperienceInternal(string id)
    {
      var result = await DeleteExperience(id);

      ExperienceDto returnResult = dataConverter.GetOkObjectResult(result);


      return returnResult; ;
    }
    // DELETE: api/Experiences/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<ExperienceDto>> DeleteExperience(string id)
    {

        try
        {



          Experience itemDeleted = await repository.Read(id);
          Notification.PostMessage("id to delete: " + itemDeleted.ID);
          await repository.Delete(itemDeleted);
          return Ok(entityConverter.ConvertToDto(itemDeleted));


        }

        catch (Exception ex)
        {
          message = "***ERROR Experience CONTROLLER" +
             "\nIN CLASS: Experience" +
             "\nWITH METHOD: DeleteExperience" +
             "\nERROR MESSAGE: " + ex.Message;
          Notification.PostMessage(message);
          return BadRequest(message);
        }
      }



    private async Task<bool> ItemExists(string id)
    {
      return await repository.Exists(id);
    }
  }
}


