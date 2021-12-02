using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Abstract;
using Portfolio.WebApp.Dtos;
using Portfolio.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Portfolio.WebApp.Controllers
{
  [EnableCors("MyAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]

    public class ProjectLinksController : ControllerBase
    {
        private readonly IProjectLinkRepository repository;
        private string message = "";
        DtoEntityConverter<ProjectLinkDto, ProjectLink> entityConverter;
        DataConverter<ProjectLinkDto> dataConverter;

        public ProjectLinksController(IProjectLinkRepository Repository)
        {
            repository = Repository;
            entityConverter = new DtoEntityConverter<ProjectLinkDto, ProjectLink>();
            dataConverter = new DataConverter<ProjectLinkDto>();
        }

        // GET: api/<ProjectLinksController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProjectLinkDto>>> GetProjectLinks()
        {
            List<ProjectLinkDto> itemDtosFound = new List<ProjectLinkDto>();
            try
            {
                List<ProjectLink> itemsFound = await repository.GetItems();
                itemsFound.ForEach(item =>
                {

                    itemDtosFound.Add(entityConverter.ConvertToDto(item));

                });
                return itemDtosFound;
            }

            catch (Exception ex)
            {
                message = "***ERROR IN CONTROLLER" +
                  "\nIN CLASS: ProjectLinkController" +
                       "\nWITH METHOD: GetProjectLinks" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }

        }

        // GET api/<ProjectLinksController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProjectLinkDto>>> GetLinksByProjectId(string id)
        {
            List<ProjectLinkDto> itemsDtosFound = new List<ProjectLinkDto>();
            try
            {
                List<ProjectLink> itemsFound = await repository.GetItemsByPC(id);
                itemsFound.ForEach(project =>
                {

                    itemsDtosFound.Add(entityConverter.ConvertToDto(project));

                });
                return itemsDtosFound;
            }

            catch (Exception ex)
            {
                message = "***ERROR IN PROJECT REQUIREMENT CONTROLLER" +
                  "\nIN CLASS: ProjectLinkController" +
                       "\nWITH METHOD: GetLinksByProjectId" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }

        }

        public async Task<ProjectLinkDto> PutProjectLinkInternal(ProjectLinkDto itemToUPdatetDto)
        {
            var result = await PutProjectLink(itemToUPdatetDto.ID, itemToUPdatetDto);

            ProjectLinkDto returnResult = dataConverter.GetOkObjectResult(result);


            return returnResult;
        }
        // PUT: api/ProjectLinks/5
        [HttpPut("{id}")]

        public async Task<ActionResult<ProjectLinkDto>> PutProjectLink(string id, [FromBody] ProjectLinkDto itemToUpdateDto)
        {
            if (id != itemToUpdateDto.ID)
            {
                return BadRequest("Bad Request, please try again");
            }

            ProjectLink itemUpdated = new ProjectLink();
            ProjectLinkDto updateditemDto = new ProjectLinkDto();

            if (await this.ItemExists(id))
            {
                try
                {
                    ProjectLink itemToUpdate = entityConverter.ConvertToCoreEntity(itemToUpdateDto);
                    itemUpdated = await repository.Update(itemToUpdate);
                    updateditemDto = entityConverter.ConvertToDto(itemUpdated);

                    return Ok(updateditemDto);
                }

                catch (Exception ex)
                {
                    message = "***ERROR IN PROJECT_LINK CONTROLLER" +
                       "\nCLASS: ProjectLink" +
                       "\nWITH METHOD: PutProjectLink()" +
                       "\nERROR MESSAGE: " + ex.Message;
                    Notification.PostMessage(message);
                    return BadRequest(message);
                }



            }
            return NotFound("ProjectLink Does not Exist");


        }

        // POST: api/ProjectLinks
        [HttpPost]

        public async Task<ActionResult<List<ProjectLinkDto>>> PostProjectLink([FromBody] ProjectLinkDto itemToPostDto)
        {
            ProjectLink itemToCreate = entityConverter.ConvertToCoreEntity(itemToPostDto);
            List<ProjectLinkDto> returnObject = new List<ProjectLinkDto>();
            if (itemToPostDto.ID != "" && await this.ItemExists(itemToPostDto.ID))
            {
                return Conflict("Project already exists");
            }
            try
            {
                List<ProjectLink> projectLinks = await repository.CreateItem(itemToCreate);
                projectLinks.ForEach(i => {
                   returnObject.Add(entityConverter.ConvertToDto(i));
                });


                return Ok(returnObject);
            }

            catch (Exception ex)
            {
                message = "***ERROR IN PROJECT_LINK CONTROLLER" +
                   "\nIN CLASS: ProjectLink" +
                       "\nWITH METHOD: PostProjectLink" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }


        }

        public async Task<List<ProjectLinkDto>> PostRangeOfLinksInternal(List<ProjectLinkDto> itemsDto)
        {
            var result = await PostRangeOfLinks(itemsDto);

            DataConverter<List<ProjectLinkDto>> dc2 = new DataConverter<List<ProjectLinkDto>>();
            List<ProjectLinkDto> returnResult = dc2.GetOkObjectResult(result);


            return returnResult;
        }
        // POST: api/Projects
        [HttpPost("postRange")]

        public async Task<ActionResult<List<ProjectLinkDto>>> PostRangeOfLinks([FromBody] List<ProjectLinkDto> itemsToPostDto)
        {

            List<ProjectLinkDto> ToPostDto = itemsToPostDto.FindAll(i => i.ID == "");

            List<ProjectLink> ToPost = new List<ProjectLink>();
            ToPostDto.ForEach(i =>
            {
                ToPost.Add(entityConverter.ConvertToCoreEntity(i));
            });



            try
            {
                List<ProjectLink> itemsCreated = await repository.CreateRange(ToPost);
                List<ProjectLinkDto> itemsCreatedDto = new List<ProjectLinkDto>();

                itemsCreated.ForEach(i =>
                {
                    itemsCreatedDto.Add(entityConverter.ConvertToDto(i));
                });
                return Ok(itemsCreatedDto);
            }

            catch (Exception ex)
            {
                message = "***ERROR IN PROJECT_LINK CONTROLLER" +
                   "\nIN CLASS: ProjectLink" +
                       "\nWITH METHOD: PostRangeOfProjectLinks" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }


        }


        // DELETE: api/ProjectLinks/5
        [HttpDelete("{id}")]

        public async Task<ActionResult<List<ProjectLinkDto> >> DeleteProjectLink(string id)
        {
            List<ProjectLink> results = new List<ProjectLink>();
            List<ProjectLinkDto> returnObject = new List<ProjectLinkDto>();
            if (id != string.Empty && await this.ItemExists(id))
            {
                try
                {
                    ProjectLinkDto itemDeleted = new ProjectLinkDto();
                    ProjectLink itemToDelete = await repository.Read(id);

                    results = await repository.DeleteItem(itemToDelete);
                    results.ForEach(i => {
                        returnObject.Add(entityConverter.ConvertToDto(i));
                    });
                    return Ok(returnObject);


                }

                catch (Exception ex)
                {
                    message = "***ERROR ProjectLink CONTROLLER" +
                       "\nIN CLASS: ProjectLink" +
                       "\nWITH METHOD: DeleteProjectLink" +
                       "\nERROR MESSAGE: " + ex.Message;
                    Notification.PostMessage(message);
                    return BadRequest(message);
                }
            }
            return NotFound("Cannot find Project to delete");
        }

        public async Task<List<ProjectLinkDto>> PutProjectLinkRangeInternal(List<ProjectLinkDto> itemToUPdatetDto)
        {
            var result = await PutProjectLinkRange(itemToUPdatetDto);
            DataConverter <List<ProjectLinkDto>> dc2 = new DataConverter<List<ProjectLinkDto>>();

            List<ProjectLinkDto> returnResult = dc2.GetOkObjectResult(result);


            return returnResult;
        }
        // PUT: api/ProjectLinks/5
        [HttpPut("updateRange")]
        public async Task<ActionResult<List<ProjectLinkDto>>> PutProjectLinkRange([FromBody] List<ProjectLinkDto> itemToUpdateDto)
        {

            List<ProjectLink> itemsToUPdate = new List<ProjectLink>();
            List<ProjectLink> itemsUpdated = new List<ProjectLink>();
            List<ProjectLinkDto> itemsUpdatedDto = new List<ProjectLinkDto>();
            List<ProjectLink> itemsToPost = new List<ProjectLink>();
            List<ProjectLink> itemsPosted = new List<ProjectLink>();
            List<ProjectLinkDto> itemsPostedDto = new List<ProjectLinkDto>();


            itemToUpdateDto.ForEach(i =>
            {
                if (!string.IsNullOrEmpty(i.ID))
                {
                    itemsToUPdate.Add(entityConverter.ConvertToCoreEntity(i));
                } else
                {
                    i.ID = Guid.NewGuid().ToString();
                    itemsToPost.Add(entityConverter.ConvertToCoreEntity(i));
                }

            });


                try
                {
                if (itemsToPost.Count > 0)
                {
                    itemsPosted = await repository.CreateRange(itemsToPost);
                    itemsPosted.ForEach(i => itemsUpdatedDto.Add(new ProjectLinkDto().ConvertToDto(i)));
                }

                    if(itemsToUPdate.Count>0)
                {

                    itemsToUPdate.ForEach(i => itemsUpdatedDto.Add(new ProjectLinkDto().ConvertToDto(i)));
                }




                    return Ok(itemsUpdatedDto);
                }

                catch (Exception ex)
                {
                    message = "***ERROR IN PROJECT_LINK CONTROLLER" +
                       "\nCLASS: ProjectLink" +
                       "\nWITH METHOD: PutProjectLink()" +
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
