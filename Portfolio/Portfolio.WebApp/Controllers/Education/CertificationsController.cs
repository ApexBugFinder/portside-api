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
  [EnableCors("MyAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
//   [Authorize]
    public class CertificationsController : ControllerBase
    {
        private readonly ICertificationRepository repository;
        private string message = "";
        DtoEntityConverter<CertificationDto, Certification> entityConverter;
        DataConverter<CertificationDto> dataConverter;

        public CertificationsController(ICertificationRepository Repository)
        {
            repository = Repository;
            entityConverter = new DtoEntityConverter<CertificationDto, Certification>();
            dataConverter = new DataConverter<CertificationDto>();
        }

        // GET: api/<CertificationsController>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<CertificationDto>>> GetCertifications()
        {
            List<CertificationDto> itemDtosFound = new List<CertificationDto>();
            try
            {
                List<Certification> itemsFound = await repository.GetItems();
                itemsFound.ForEach(item =>
                {

                    itemDtosFound.Add(entityConverter.ConvertToDto(item));

                });
                return itemDtosFound;
            }

            catch (Exception ex)
            {
                message = "***ERROR IN CONTROLLER" +
                  "\nIN CLASS: CertificationController" +
                       "\nWITH METHOD: GetCertifications" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }

        }

        // GET api/<CertificationsController>/5
        [AllowAnonymous]
        [HttpGet("all/{id}")]
        public async Task<ActionResult<List<CertificationDto>>> GetCertificationsByProjectCreatorId(string id)
        {
            List<CertificationDto> itemsDtosFound = new List<CertificationDto>();
            try
            {
                List<Certification> itemsFound = await repository.GetItemsByPC(id);
                itemsFound.ForEach(project =>
                {
                        itemsDtosFound.Add(entityConverter.ConvertToDto(project));
                });
                return itemsDtosFound;
            }

            catch (Exception ex)
            {
                message = "***ERROR IN  CONTROLLER" +
                  "\nIN CLASS: CertificationController" +
                       "\nWITH METHOD: GetCertificationsByProjectCreatorId" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }

        }

        public async Task<CertificationDto> PutCertificationInternal(CertificationDto itemToUPdatetDto)
        {
            var result = await PutCertification(itemToUPdatetDto.ID, itemToUPdatetDto);

            CertificationDto returnResult = dataConverter.GetOkObjectResult(result);


            return returnResult;
        }
        // PUT: api/Certifications/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult<CertificationDto>> PutCertification(string id, [FromBody] CertificationDto itemToUpdateDto)
        {
            if (id != itemToUpdateDto.ID)
            {
                return BadRequest("Bad Request, please try again");
            }

            Certification itemUpdated = new Certification();
            CertificationDto updateditemDto = new CertificationDto();

            if (await this.ItemExists(id))
            {
                try
                {
                    Certification itemToUpdate = entityConverter.ConvertToCoreEntity(itemToUpdateDto);
                    itemUpdated = await repository.Update(itemToUpdate);
                    updateditemDto = entityConverter.ConvertToDto(itemUpdated);

                    return Ok(updateditemDto);
                }

                catch (Exception ex)
                {
                    message = "***ERROR IN CONTROLLER" +
                       "\nCLASS: Certification" +
                       "\nWITH METHOD: PutCertification()" +
                       "\nERROR MESSAGE: " + ex.Message;
                    Notification.PostMessage(message);
                    return BadRequest(message);
                }

            }
            return NotFound("Certification Does not Exist");

        }
        public async Task<List<CertificationDto>> PostCertificationInternal(CertificationDto itemDto)
        {
            var result = await PostCertification(itemDto);
            DataConverter<List<CertificationDto>> dc2 = new DataConverter<List<CertificationDto>>();
            List<CertificationDto> returnResult = dc2.GetOkObjectResult(result);
            return returnResult;
        }
    // POST: api/Certifications
    [HttpPost("new")]
        public async Task<ActionResult<List<CertificationDto>>> PostCertification([FromBody] CertificationDto itemToPostDto)
        {
            Certification itemToCreate = entityConverter.ConvertToCoreEntity(itemToPostDto);
            List<Certification> results = new List<Certification>();

            if (itemToPostDto.ID != "" && await this.ItemExists(itemToPostDto.ID))
            {
                return Conflict("Project already exists");
            }
            try
            {
                Certification itemCreated = await repository.Create(itemToCreate);
                CertificationDto itemCreatedDto = entityConverter.ConvertToDto(itemCreated);

                return Ok(itemCreatedDto);
            }

            catch (Exception ex)
            {
                message = "***ERROR IN CONTROLLER" +
                   "\nIN CLASS: Certification" +
                       "\nWITH METHOD: PostCertification" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }


        }



        public async Task<CertificationDto> DeleteCertificationInternal(string id)
        {
            var result = await DeleteCertification(id);

            CertificationDto returnResult = dataConverter.GetOkObjectResult(result);


            return returnResult; ;
        }
        // DELETE: api/Certifications/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CertificationDto>> DeleteCertification(string id)
        {
            if (id != string.Empty && await this.ItemExists(id))
            {
                try
                {
                    CertificationDto itemDeleted = new CertificationDto();
                    Certification itemToDelete = await repository.Read(id);

                    await repository.Delete(itemToDelete.ID);
                    return Ok(entityConverter.ConvertToDto(itemToDelete));


                }

                catch (Exception ex)
                {
                    message = "***ERROR Certification CONTROLLER" +
                       "\nIN CLASS: Certification" +
                       "\nWITH METHOD: DeleteCertification" +
                       "\nERROR MESSAGE: " + ex.Message;
                    Notification.PostMessage(message);
                    return BadRequest(message);
                }
            }
            return NotFound("Cannot find Certifcation to delete");
        }



        private async Task<bool> ItemExists(string id)
        {
            return await repository.Exists(id);
        }
    }
}
