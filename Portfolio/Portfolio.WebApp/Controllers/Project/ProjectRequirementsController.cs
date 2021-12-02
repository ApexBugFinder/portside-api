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
    [Route("api/[controller]")]
  [EnableCors("MyAllowSpecificOrigins")]
    [ApiController]

    public class ProjectRequirementsController : ControllerBase
    {

        private readonly IProjectRequirementRepository repository;
        private string message = "";
        DtoEntityConverter<ProjectRequirementDto, ProjectRequirement> entityConverter;
        DataConverter<ProjectRequirementDto> dataConverter;




        public ProjectRequirementsController(IProjectRequirementRepository Repository)
        {
            repository = Repository;
            entityConverter = new DtoEntityConverter<ProjectRequirementDto, ProjectRequirement>();
            dataConverter = new DataConverter<ProjectRequirementDto>();
        }

        // GET: api/<ProjectRequirementsController>
        [HttpGet]
        public async Task<ActionResult<List<ProjectRequirementDto>>> GetProjectRequirements()
        {
            List<ProjectRequirementDto> itemDtosFound = new List<ProjectRequirementDto>();
            try
            {
                List<ProjectRequirement> itemsFound = await repository.GetItems();
                itemsFound.ForEach(item =>
                {

                    itemDtosFound.Add(entityConverter.ConvertToDto(item));

                });
                return itemDtosFound;
            }

            catch (Exception ex)
            {
                message = "***ERROR IN CONTROLLER" +
                  "\nIN CLASS: ProjectRequirementController" +
                       "\nWITH METHOD: GetProjectRequirements" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }

        }

        public async Task<ProjectRequirementDto> GetRequirementByIdInternal(ProjectRequirementDto itemToDeleteDto)
        {
            var result = await GetRequirementsById(itemToDeleteDto.ID);


            ProjectRequirementDto returnResult = dataConverter.GetOkObjectResult(result);


            return returnResult;
        }
        // GET api/<ProjectRequirementsController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProjectRequirementDto>> GetRequirementsById(string id)
        {
            ProjectRequirementDto itemsDtoFound = new ProjectRequirementDto();
            try
            {
                ProjectRequirement itemsFound = await repository.Read(id);
                itemsDtoFound = entityConverter.ConvertToDto(itemsFound);
                return itemsDtoFound;
            }

            catch (Exception ex)
            {
                message = "***ERROR IN PROJECT REQUIREMENT CONTROLLER" +
                  "\nIN CLASS: ProjectRequirementController" +
                       "\nWITH METHOD: GetRequirementsById" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }

        }

        [HttpGet("project/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProjectRequirementDto>>> GetRequirementsByProjectId(string id)
        {
            List<ProjectRequirementDto> itemsDtosFound = new List<ProjectRequirementDto>();
            try
            {
                List<ProjectRequirement> itemsFound = await repository.GetItemsByPC(id);
                itemsFound.ForEach(project =>
                {

                    itemsDtosFound.Add(entityConverter.ConvertToDto(project));

                });
                return itemsDtosFound;
            }

            catch (Exception ex)
            {
                message = "***ERROR IN PROJECT REQUIREMENT CONTROLLER" +
                  "\nIN CLASS: ProjectRequirementController" +
                       "\nWITH METHOD: GetRequirementsByProjectId" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }

        }

        public async Task<List<ProjectRequirementDto>> PutProjectRequirementRangeInternal(List<ProjectRequirementDto> itemToUPdatetDto)
        {
            var result = await PutProjectRequirementRange( itemToUPdatetDto);

            DataConverter<List<ProjectRequirementDto>> dc2 = new DataConverter<List<ProjectRequirementDto>>();
            List<ProjectRequirementDto> returnResult = dc2.GetOkObjectResult(result);


            return returnResult;
        }
        // PUT: api/ProjectRequirements/5
        [HttpPut("updateRange")]
        public async Task<ActionResult<List<ProjectRequirementDto>>> PutProjectRequirementRange([FromBody] List<ProjectRequirementDto> rangeToUpdateDto)
        {

            // SEPARATE RANGE INTO 3 RANGES
            // ADD RANGE, REMOVE RANGE, AND NO-CHANGE-RANGE
            List<ProjectRequirementDto> addRange = rangeToUpdateDto.Where(i => i.EditState == "add").ToList();
            List<ProjectRequirementDto> rangeAdded = new List<ProjectRequirementDto>();
            List<ProjectRequirementDto> deleteRange = rangeToUpdateDto.Where(i => i.EditState == "remove").ToList();
            List<ProjectRequirementDto> rangeDeleted = new List<ProjectRequirementDto>();
            List<ProjectRequirementDto> noChangeRange = rangeToUpdateDto.Where(i => i.EditState == "ok").ToList();

            if (addRange.Count > 0)
            {
                rangeAdded = await PostRangeOfRequirementsInternal(addRange);
                message = rangeAdded.Count + " project Requirements added to the DB";
                Notification.PostMessage(message);
            }

            if (deleteRange.Count > 0)
            {
                rangeDeleted = await DeleteProjectRequirementRangeInternal(deleteRange);
                message = rangeDeleted.Count + " project Requirements added to the DB";
                Notification.PostMessage(message);
            }

            if (noChangeRange.Count > 0)
            {
                message = noChangeRange.Count + " project Requirements were not changed";
                Notification.PostMessage(message);
            }

            List<ProjectRequirementDto> itemsUpdated = new List<ProjectRequirementDto>();
            itemsUpdated.AddRange(rangeAdded);
            itemsUpdated.AddRange(noChangeRange);
            return itemsUpdated;


        }



        public async Task<ProjectRequirementDto> PutProjectRequirementInternal(ProjectRequirementDto itemToUPdatetDto)
        {
            var result = await PutProjectRequirement(itemToUPdatetDto.ID, itemToUPdatetDto);

            ProjectRequirementDto returnResult = dataConverter.GetOkObjectResult(result);


            return returnResult;
        }
        // PUT: api/ProjectRequirements/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectRequirementDto>> PutProjectRequirement(string id, [FromBody] ProjectRequirementDto itemToUpdateDto)
        {


            if (id != itemToUpdateDto.ID)
            {
                return BadRequest("Bad Request, please try again");
            }

            ProjectRequirement itemUpdated = new ProjectRequirement();
            ProjectRequirementDto updateditemDto = new ProjectRequirementDto();

            if (await this.ItemExists(id))
            {
                try
                {
                    ProjectRequirement itemToUpdate = entityConverter.ConvertToCoreEntity(itemToUpdateDto);
                    itemUpdated = await repository.Update(itemToUpdate);
                    updateditemDto = entityConverter.ConvertToDto(itemUpdated);

                    return Ok(updateditemDto);
                }

                catch (Exception ex)
                {
                    message = "***ERROR IN PROJECT_REQUIREMENT CONTROLLER" +
                       "\nCLASS: ProjectRequirement" +
                       "\nWITH METHOD: PutProjectRequirement()" +
                       "\nERROR MESSAGE: " + ex.Message;
                    Notification.PostMessage(message);
                    return BadRequest(message);
                }



            }
            return NotFound("ProjectRequirement Does not Exist");


        }
        public async Task<ProjectRequirementDto> PostProjectRequirementInternal(ProjectRequirementDto itemDto)
        {
            var result = await PostProjectRequirement(itemDto);

            ProjectRequirementDto returnResult = dataConverter.GetOkObjectResult(result);


            return returnResult;
        }
        // POST: api/Projects
        [HttpPost]
        public async Task<ActionResult<ProjectRequirementDto>> PostProjectRequirement([FromBody] ProjectRequirementDto itemToPostDto)
        {
            ProjectRequirement itemToCreate = entityConverter.ConvertToCoreEntity(itemToPostDto);

            if (itemToPostDto.ID != "" && await this.ItemExists(itemToPostDto.ID))
            {
                return Conflict("Project already exists");
            }
            try
            {
                ProjectRequirement itemCreated = await repository.Create(itemToCreate);
                ProjectRequirementDto itemCreatedDto = entityConverter.ConvertToDto(itemCreated);

                return Ok(itemCreatedDto);
            }

            catch (Exception ex)
            {
                message = "***ERROR IN PROJECT_REQUIREMENT CONTROLLER" +
                   "\nIN CLASS: ProjectRequirement" +
                       "\nWITH METHOD: PostProjectRequirement" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }


        }


        public async Task<List<ProjectRequirementDto>> PostRangeOfRequirementsInternal(List<ProjectRequirementDto> itemsDto)
        {
            var result = await PostRangeOfRequirements(itemsDto);

            DataConverter < List < ProjectRequirementDto >> dc2 = new DataConverter<List<ProjectRequirementDto>>();
            List<ProjectRequirementDto> returnResult = dc2.GetOkObjectResult(result);


            return returnResult;
        }
        // POST: api/Projects
        [HttpPost("range")]
        public async Task<ActionResult<List<ProjectRequirementDto>>> PostRangeOfRequirements([FromBody] List<ProjectRequirementDto> itemsToPostDto)
        {

            List<ProjectRequirementDto> ToPostDto = itemsToPostDto.FindAll(i => i.EditState == "add");

            List<ProjectRequirement> ToPost = new List<ProjectRequirement>();
            ToPostDto.ForEach(i =>
            {
                ToPost.Add(entityConverter.ConvertToCoreEntity(i));
            });



            try
            {
                List<ProjectRequirement> itemsCreated = await repository.CreateRange(ToPost);
                List<ProjectRequirementDto> itemsCreatedDto = new List<ProjectRequirementDto>();

                itemsCreated.ForEach(i =>
                {
                    itemsCreatedDto.Add(entityConverter.ConvertToDto(i));
                });
                return Ok(itemsCreatedDto);
            }

            catch (Exception ex)
            {
                message = "***ERROR IN PROJECT_REQUIREMENT CONTROLLER" +
                   "\nIN CLASS: ProjectRequirement" +
                       "\nWITH METHOD: PostRangeOfProjectRequirements" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }


        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<ProjectRequirementDto>>> DeleteProjectRequirement(string id)
        {
            List<ProjectRequirement> results = new List<ProjectRequirement>();
            List<ProjectRequirementDto> returnObject = new List<ProjectRequirementDto>();
            if (id != string.Empty && await this.ItemExists(id))
            {
                try
                {

                    ProjectRequirement itemToDelete = await repository.Read(id);

                    results = await repository.DeleteItem(itemToDelete);

                    if (results.Count >0) {
                        results.ForEach(i => {
                            returnObject.Add(entityConverter.ConvertToDto(i));
                        });
                    }
                    return Ok(returnObject);


                }

                catch (Exception ex)
                {
                    message = "***ERROR PROJECTREQUIREMENT CONTROLLER" +
                       "\nIN CLASS: ProjectRequirement" +
                       "\nWITH METHOD: DeleteProjectRequirement" +
                       "\nERROR MESSAGE: " + ex.Message;
                    Notification.PostMessage(message);
                    return BadRequest(message);
                }
            }
            return NotFound("Cannot find Project to delete");
        }

        public async Task<List<ProjectRequirementDto>> DeleteProjectRequirementRangeInternal(List<ProjectRequirementDto> rangeToDelete)
        {
            var result = await DeleteProjectRequirementRange(rangeToDelete);

            DataConverter < List < ProjectRequirementDto> > dc2 = new DataConverter<List<ProjectRequirementDto>>();
            List<ProjectRequirementDto> returnResult = dc2.GetOkObjectResult(result);


            return returnResult; ;
        }
        // DELETE: api/Projects/5
        [HttpDelete("range")]
        public async Task<ActionResult<List<ProjectRequirementDto>>> DeleteProjectRequirementRange(List<ProjectRequirementDto> range)
        {

                try
                {
                List<ProjectRequirementDto> itemsDeletedDto = new List<ProjectRequirementDto>();
                List<ProjectRequirement> itemsToDelete = new List<ProjectRequirement>();
                List<ProjectRequirement> deleted = new List<ProjectRequirement>();
                range.ForEach(async item =>
                    {

                        ProjectRequirement itemToDelete = await repository.Read(item.ID);
                        try
                        {
                            if (itemToDelete.ID != null)
                            {
                                itemsToDelete.Add(itemToDelete);
                            }
                        } catch (NullReferenceException ex)
                        {
                            message = itemToDelete.ID + " does not exist";
                        }

                    });


                     await repository.DeleteRange(itemsToDelete);


                itemsToDelete.ForEach(i =>
                {
                    itemsDeletedDto.Add(entityConverter.ConvertToDto(i));
                });
                    return Ok(itemsDeletedDto);


                }

                catch (Exception ex)
                {
                    message = "***ERROR PROJECTREQUIREMENT CONTROLLER" +
                       "\nIN CLASS: ProjectRequirement" +
                       "\nWITH METHOD: DeleteProjectRequirement" +
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
