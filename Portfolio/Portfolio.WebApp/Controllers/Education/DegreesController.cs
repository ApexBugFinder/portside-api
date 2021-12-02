using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.PorfolioDomain.Core.Entities;
using Portfolio.WebApp.Abstract;
using Portfolio.WebApp.Dtos;
using Portfolio.WebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Portfolio.WebApp.Controllers.Education
{
    [Route("api/[controller]")]
    [ApiController]

  [EnableCors("MyAllowSpecificOrigins")]
    public class DegreesController : ControllerBase
    {
        private readonly IDegreeRepository repository;
        private string message = "";
        DtoEntityConverter<DegreeDto, Degree> entityConverter;
        DataConverter<DegreeDto> dataConverter;

        public DegreesController(IDegreeRepository Repository)
        {
            repository = Repository;
            entityConverter = new DtoEntityConverter<DegreeDto, Degree>();
            dataConverter = new DataConverter<DegreeDto>();
        }

        // GET: api/<DegreesController>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<DegreeDto>>> GetDegrees()
        {
            List<DegreeDto> itemDtosFound = new List<DegreeDto>();
            try
            {
                List<Degree> itemsFound = await repository.GetItems();
                itemsFound.ForEach(item =>
                {

                    itemDtosFound.Add(entityConverter.ConvertToDto(item));

                });
                return itemDtosFound;
            }

            catch (Exception ex)
            {
                message = "***ERROR IN CONTROLLER" +
                  "\nIN CLASS: DegreeController" +
                       "\nWITH METHOD: GetDegrees" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }

        }

        // GET api/<DegreesController>/5
        [AllowAnonymous]
        [HttpGet("all/{id}")]
        public async Task<ActionResult<List<DegreeDto>>> GetDegreesByProjectCreatorId(string id)
        {
            List<DegreeDto> itemsDtosFound = new List<DegreeDto>();
            try
            {
                List<Degree> itemsFound = await repository.GetItemsByPC(id);
                itemsFound.ForEach(project =>
                {

                    itemsDtosFound.Add(entityConverter.ConvertToDto(project));

                });
                return itemsDtosFound;
            }

            catch (Exception ex)
            {
                message = "***ERROR IN CONTROLLER" +
                  "\nIN CLASS: DegreeController" +
                       "\nWITH METHOD: GetLinksByProjectId" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }

        }

        public async Task<DegreeDto> PutDegreeInternal(DegreeDto itemToUPdatetDto)
        {
            var result = await PutDegree(itemToUPdatetDto.ID, itemToUPdatetDto);

            DegreeDto returnResult = dataConverter.GetOkObjectResult(result);


            return returnResult;
        }
        // PUT: api/Degrees/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult<DegreeDto>> PutDegree(string id, [FromBody] DegreeDto itemToUpdateDto)
        {
            if (id != itemToUpdateDto.ID)
            {
                return BadRequest("Bad Request, please try again");
            }

            Degree itemUpdated = new Degree();
            DegreeDto updateditemDto = new DegreeDto();

            if (await this.ItemExists(id))
            {
                try
                {
                    Degree itemToUpdate = entityConverter.ConvertToCoreEntity(itemToUpdateDto);
                    itemUpdated = await repository.Update(itemToUpdate);
                    updateditemDto = entityConverter.ConvertToDto(itemUpdated);

                    return Ok(updateditemDto);
                }

                catch (Exception ex)
                {
                    message = "***ERROR IN CONTROLLER" +
                       "\nCLASS: Degree" +
                       "\nWITH METHOD: PutDegree()" +
                       "\nERROR MESSAGE: " + ex.Message;
                    Notification.PostMessage(message);
                    return BadRequest(message);
                }



            }
            return NotFound("Degree Does not Exist");


        }
        public async Task<DegreeDto> PostDegreeInternal(DegreeDto itemDto)
        {
            var result = await PostDegree(itemDto);

            DegreeDto returnResult = dataConverter.GetOkObjectResult(result);


            return returnResult;
        }
        // POST: api/Degrees
        [HttpPost("new")]
        public async Task<ActionResult<DegreeDto>> PostDegree([FromBody] DegreeDto itemToPostDto)
        {
            Degree itemToCreate = entityConverter.ConvertToCoreEntity(itemToPostDto);

            if (itemToPostDto.ID != "" && await this.ItemExists(itemToPostDto.ID))
            {
                return Conflict("Degree already exists");
            }
            try
            {
                Degree itemCreated = await repository.Create(itemToCreate);
                DegreeDto itemCreatedDto = entityConverter.ConvertToDto(itemCreated);

                return Ok(itemCreatedDto);
            }

            catch (Exception ex)
            {
                message = "***ERROR IN CONTROLLER" +
                   "\nIN CLASS: Degree" +
                       "\nWITH METHOD: PostDegree" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }


        }



        public async Task<DegreeDto> DeleteDegreeInternal(string id)
        {
            var result = await DeleteDegree(id);

            DegreeDto returnResult = dataConverter.GetOkObjectResult(result);


            return returnResult; ;
        }
        // DELETE: api/Degrees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DegreeDto>> DeleteDegree(string id)
        {
            if (id != string.Empty && await this.ItemExists(id))
            {
                try
                {
                    DegreeDto itemDeleted = new DegreeDto();
                    Degree itemToDelete = await repository.Read(id);

                    await repository.Delete(itemToDelete.ID);
                    return Ok(entityConverter.ConvertToDto(itemToDelete));


                }

                catch (Exception ex)
                {
                    message = "***ERROR  CONTROLLER" +
                       "\nIN CLASS: Degree" +
                       "\nWITH METHOD: DeleteDegree" +
                       "\nERROR MESSAGE: " + ex.Message;
                    Notification.PostMessage(message);
                    return BadRequest(message);
                }
            }
            return NotFound("Cannot find Degree to delete");
        }



        private async Task<bool> ItemExists(string id)
        {
            return await repository.Exists(id);
        }
    }
}
