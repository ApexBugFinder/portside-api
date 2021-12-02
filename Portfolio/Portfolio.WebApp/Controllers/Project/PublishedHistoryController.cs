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
    [Authorize]
    public class PublishedHistoryController : ControllerBase
    {
        private readonly IPublishedHistoryRepository repository;
        private string message = "";
        DtoEntityConverter<PublishedHistoryDto, PublishedHistory> entityConverter;
        DataConverter<PublishedHistoryDto> dataConverter;

        public PublishedHistoryController(IPublishedHistoryRepository Repository)
        {
            repository = Repository;
            entityConverter = new DtoEntityConverter<PublishedHistoryDto, PublishedHistory>();
            dataConverter = new DataConverter<PublishedHistoryDto>();
        }

        // GET: api/<PublishedHistorysController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<PublishedHistoryDto>>> GetPublishedHistorys()
        {
            List<PublishedHistoryDto> itemDtosFound = new List<PublishedHistoryDto>();
            try
            {
                List<PublishedHistory> itemsFound = await repository.GetItems();
                itemsFound.ForEach(item =>
                {

                    itemDtosFound.Add(entityConverter.ConvertToDto(item));

                });
                return itemDtosFound;
            }

            catch (Exception ex)
            {
                message = "***ERROR IN CONTROLLER" +
                  "\nIN CLASS: PublishedHistoryController" +
                       "\nWITH METHOD: GetPublishedHistorys" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }

        }

        // GET api/<PublishedHistorysController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PublishedHistoryDto>>> GetLinksByProjectId(string id)
        {
            List<PublishedHistoryDto> itemsDtosFound = new List<PublishedHistoryDto>();
            try
            {
                List<PublishedHistory> itemsFound = await repository.GetItemsByPC(id);
                itemsFound.ForEach(project =>
                {

                    itemsDtosFound.Add(entityConverter.ConvertToDto(project));

                });
                return itemsDtosFound;
            }

            catch (Exception ex)
            {
                message = "***ERROR IN PROJECT Published History CONTROLLER" +
                  "\nIN CLASS: PublishedHistoryController" +
                       "\nWITH METHOD: GetPublishedHistoryByProjectId" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }

        }

        public async Task<PublishedHistoryDto> PutPublishedHistoryInternal(PublishedHistoryDto itemToUPdatetDto)
        {
            var result = await PutPublishedHistory(itemToUPdatetDto.ID, itemToUPdatetDto);

            PublishedHistoryDto returnResult = dataConverter.GetOkObjectResult(result);


            return returnResult;
        }
        // PUT: api/PublishedHistorys/5
        [HttpPut("{id}")]
        public async Task<ActionResult<PublishedHistoryDto>> PutPublishedHistory(string id, [FromBody] PublishedHistoryDto itemToUpdateDto)
        {
            if (id != itemToUpdateDto.ID)
            {
                return BadRequest("Bad Request, please try again");
            }

            PublishedHistory itemUpdated = new PublishedHistory();
            PublishedHistoryDto updateditemDto = new PublishedHistoryDto();

            if (await this.ItemExists(id))
            {
                try
                {
                    PublishedHistory itemToUpdate = entityConverter.ConvertToCoreEntity(itemToUpdateDto);
                    itemUpdated = await repository.Update(itemToUpdate);
                    updateditemDto = entityConverter.ConvertToDto(itemUpdated);

                    return Ok(updateditemDto);
                }

                catch (Exception ex)
                {
                    message = "***ERROR IN PROJECT Published History CONTROLLER" +
                       "\nCLASS: PublishedHistory" +
                       "\nWITH METHOD: PutPublishedHistory()" +
                       "\nERROR MESSAGE: " + ex.Message;
                    Notification.PostMessage(message);
                    return BadRequest(message);
                }



            }
            return NotFound("PublishedHistory Does not Exist");


        }
        public async Task<PublishedHistoryDto> PostPublishedHistoryInternal(PublishedHistoryDto itemDto)
        {
            var result = await PostPublishedHistory(itemDto);

            PublishedHistoryDto returnResult = dataConverter.GetOkObjectResult(result);


            return returnResult;
        }
        // POST: api/PublishedHistorys
        [HttpPost]
        public async Task<ActionResult<PublishedHistoryDto>> PostPublishedHistory([FromBody] PublishedHistoryDto itemToPostDto)
        {
            PublishedHistory itemToCreate = entityConverter.ConvertToCoreEntity(itemToPostDto);

            if (itemToPostDto.ID != "" && await this.ItemExists(itemToPostDto.ID))
            {
                return Conflict("Project already exists");
            }
            try
            {
                PublishedHistory itemCreated = await repository.Create(itemToCreate);
                PublishedHistoryDto itemCreatedDto = entityConverter.ConvertToDto(itemCreated);

                return Ok(itemCreatedDto);
            }

            catch (Exception ex)
            {
                message = "***ERROR IN PROJECT Published History CONTROLLER" +
                   "\nIN CLASS: PublishedHistory" +
                       "\nWITH METHOD: PostPublishedHistory" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }


        }



        public async Task<PublishedHistoryDto> DeletePublishedHistoryInternal(string id)
        {
            var result = await DeletePublishedHistory(id);

            PublishedHistoryDto returnResult = dataConverter.GetOkObjectResult(result);


            return returnResult; ;
        }
        // DELETE: api/PublishedHistorys/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PublishedHistoryDto>> DeletePublishedHistory(string id)
        {
            if (id != string.Empty && await this.ItemExists(id))
            {
                try
                {
                    PublishedHistoryDto itemDeleted = new PublishedHistoryDto();
                    PublishedHistory itemToDelete = await repository.Read(id);

                    await repository.Delete(itemToDelete.ID);
                    return Ok(entityConverter.ConvertToDto(itemToDelete));


                }

                catch (Exception ex)
                {
                    message = "***ERROR PublishedHistory CONTROLLER" +
                       "\nIN CLASS: PublishedHistory" +
                       "\nWITH METHOD: DeletePublishedHistory" +
                       "\nERROR MESSAGE: " + ex.Message;
                    Notification.PostMessage(message);
                    return BadRequest(message);
                }
            }
            return NotFound("Cannot find Project to delete");
        }



        private async Task<bool> ItemExists(string id)
        {
            return await repository.Exists(id);
        }
    }
}
