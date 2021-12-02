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

    public class RolesController : ControllerBase
    {
        private readonly IRolesRepository repository;
        private string message = "";
        DtoEntityConverter<RoleDto, Role> entityConverter;
        DataConverter<RoleDto> dataConverter;

        public RolesController(IRolesRepository Repository)
        {
            repository = Repository;
            entityConverter = new DtoEntityConverter<RoleDto, Role>();
            dataConverter = new DataConverter<RoleDto>();
        }

        // GET: api/<RolesController>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<RoleDto>>> GetRoles()
        {
            List<RoleDto> itemDtosFound = new List<RoleDto>();
            try
            {
                List<Role> itemsFound = await repository.GetItems();
                itemsFound.ForEach(item =>
                {

                    itemDtosFound.Add(entityConverter.ConvertToDto(item));

                });
                return itemDtosFound;
            }

            catch (Exception ex)
            {
                message = "***ERROR IN CONTROLLER" +
                  "\nIN CLASS: RoleController" +
                       "\nWITH METHOD: GetRoles" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }

        }


        public async Task<List<RoleDto>> GetRolesByExperienceIdInternal(string item)
        {
            var result = await GetRolesByExperienceId(item);

            DataConverter<List<RoleDto>> dc2 = new DataConverter<List<RoleDto>>();
            List<RoleDto> returnResult = dc2.GetOkObjectResult(result);


            return returnResult;
        }

        // GET api/<RolesController>/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<RoleDto>>> GetRolesByExperienceId(string id)
        {
            List<RoleDto> itemsDtosFound = new List<RoleDto>();
            try
            {
                List<Role> itemsFound = await repository.GetItemsByPC(id);
                itemsFound.ForEach(roles =>
                {
                    var role = entityConverter.ConvertToDto(roles);
                    role.EditState = "ok";
                    itemsDtosFound.Add(role);

                });
                return itemsDtosFound;
            }

            catch (Exception ex)
            {
                message = "***ERROR IN PROJECT REQUIREMENT CONTROLLER" +
                  "\nIN CLASS: RoleController" +
                       "\nWITH METHOD: GetLinksByProjectId" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }

        }

        public async Task<RoleDto> PutRoleInternal(RoleDto itemToUPdatetDto)
        {
            var result = await PutRole(itemToUPdatetDto.ID, itemToUPdatetDto);

            RoleDto returnResult = dataConverter.GetOkObjectResult(result);


            return returnResult;
        }
        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<ActionResult<RoleDto>> PutRole(string id, [FromBody] RoleDto itemToUpdateDto)
        {
            if (id != itemToUpdateDto.ID)
            {
                return BadRequest("Bad Request, please try again");
            }

            Role itemUpdated = new Role();
            RoleDto updateditemDto = new RoleDto();

            if (await this.ItemExists(id))
            {
                try
                {
                    Role itemToUpdate = entityConverter.ConvertToCoreEntity(itemToUpdateDto);
                    itemUpdated = await repository.Update(itemToUpdate);
                    updateditemDto = entityConverter.ConvertToDto(itemUpdated);

                    return Ok(updateditemDto);
                }

                catch (Exception ex)
                {
                    message = "***ERROR IN PROJECT_REQUIREMENT CONTROLLER" +
                       "\nCLASS: Role" +
                       "\nWITH METHOD: PutRole()" +
                       "\nERROR MESSAGE: " + ex.Message;
                    Notification.PostMessage(message);
                    return BadRequest(message);
                }



            }
            return NotFound("Role Does not Exist");


        }

        public async Task<List<RoleDto>> PutRoleRangeInternal(List<RoleDto> itemToUPdatetDto)
        {
            var result = await PutRoleRange(itemToUPdatetDto);

            DataConverter<List<RoleDto>> dc2 = new DataConverter<List<RoleDto>>();
            List<RoleDto> returnResult = dc2.GetOkObjectResult(result);


            return returnResult;
        }
        // PUT: api/Roles/updateRange
        [HttpPut("updateRange")]
        public async Task<ActionResult<List<RoleDto>>> PutRoleRange([FromBody] List<RoleDto> rangeToUpdateDto)
        {

            // SEPARATE RANGE INTO 3 RANGES
            // ADD RANGE, REMOVE RANGE, AND NO-CHANGE-RANGE
            List<RoleDto> addRange = rangeToUpdateDto.Where(i => i.EditState == "add").ToList();
            List<RoleDto> rangeAdded = new List<RoleDto>();
            List<RoleDto> deleteRange = rangeToUpdateDto.Where(i => i.EditState == "remove").ToList();
            List<RoleDto> rangeDeleted = new List<RoleDto>();
            List<RoleDto> noChangeRange = rangeToUpdateDto.Where(i => i.EditState == "ok").ToList();

            if (addRange.Count > 0)
            {
                rangeAdded = await PostRangeOfRolesInternal(addRange);
                message = rangeAdded.Count + " project Roles added to the DB";
                Notification.PostMessage(message);
            }

            if (deleteRange.Count > 0)
            {
                rangeDeleted = await DeleteRoleRangeInternal(deleteRange);
                message = rangeDeleted.Count + " project Roles added to the DB";
                Notification.PostMessage(message);
            }

            if (noChangeRange.Count > 0)
            {
                message = noChangeRange.Count + " project Roles were not changed";
                Notification.PostMessage(message);
            }

            List<RoleDto> itemsUpdated = new List<RoleDto>();
            itemsUpdated.AddRange(rangeAdded);
            itemsUpdated.AddRange(noChangeRange);
            return itemsUpdated;


        }

        public async Task<List<RoleDto>> PostRoleInternal(RoleDto itemDto)
        {
            var result = await PostRole(itemDto);
            List<RoleDto> returnResults = new List<RoleDto>();
            DataConverter<List<RoleDto>> dc2 = new DataConverter<List<RoleDto>>();
            List<RoleDto> returnResult = dc2.GetOkObjectResult(result);


            return returnResult;
        }
        // POST: api/Roles
        [HttpPost("new")]
        public async Task<ActionResult<List<RoleDto>>> PostRole([FromBody] RoleDto itemToPostDto)
        {
            Role itemToCreate = entityConverter.ConvertToCoreEntity(itemToPostDto);
            List<RoleDto> results = new List <RoleDto>();

            if (await this.ItemExists(itemToPostDto.ID))
            {
                return Conflict("Role already exists");
            }
            try
            {
                List<Role> RolesExist = await repository.Create(itemToCreate);

                RolesExist.ForEach(i => {
                    results.Add(entityConverter.ConvertToDto(i));
                });



                return Ok(results);
            }

            catch (Exception ex)
            {
                message = "***ERROR IN PROJECT_REQUIREMENT CONTROLLER" +
                   "\nIN CLASS: Role" +
                       "\nWITH METHOD: PostRole" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }


        }

        public async Task<List<RoleDto>> PostRangeOfRolesInternal(List<RoleDto> itemsDto)
        {
            var result = await PostRangeOfRoles(itemsDto);

            DataConverter<List<RoleDto>> dc2 = new DataConverter<List<RoleDto>>();
            List<RoleDto> returnResult = dc2.GetOkObjectResult(result);


            return returnResult;
        }
        // POST: api/Roles/postRange
        [HttpPost("postRange")]
        public async Task<ActionResult<List<RoleDto>>> PostRangeOfRoles([FromBody] List<RoleDto> itemsToPostDto)
        {

            List<RoleDto> ToPostDto = itemsToPostDto.FindAll(i => i.EditState == "add");

            List<Role> ToPost = new List<Role>();
            ToPostDto.ForEach(i =>
            {
                i.ID = Guid.NewGuid().ToString();
                ToPost.Add(entityConverter.ConvertToCoreEntity(i));
            });



            try
            {
                List<Role> itemsCreated = await repository.CreateRange(ToPost);
                List<RoleDto> itemsCreatedDto = new List<RoleDto>();

                itemsCreated.ForEach(i =>
                {
                    itemsCreatedDto.Add(entityConverter.ConvertToDto(i));
                });
                return Ok(itemsCreatedDto);
            }

            catch (Exception ex)
            {
                message = "***ERROR IN ROLE CONTROLLER" +
                   "\nIN CLASS: Role" +
                       "\nWITH METHOD: PostRangeOfRoles" +
                   "\nERROR MESSAGE: " + ex.Message;
                Notification.PostMessage(message);
                return BadRequest(message);
            }


        }

        public async Task<List<RoleDto>> DeleteRoleInternal(string id)
        {
            var result = await DeleteRole(id);

      DataConverter<List<RoleDto>> dc2 = new DataConverter<List<RoleDto>>();
      List<RoleDto> returnResult = dc2.GetOkObjectResult(result);


            return returnResult; ;
        }
        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<RoleDto>>> DeleteRole(string id)
        {
            List<Role> remainingRoles = new List<Role>();
            List<RoleDto> returnObject = new List<RoleDto>();
            if (id != string.Empty && await this.ItemExists(id))
            {
                try
                {


                    remainingRoles =await repository.Delete(id);
                    // return Ok(entityConverter.ConvertToDto(itemToDelete));
                    remainingRoles.ForEach(i => {
                            returnObject.Add(entityConverter.ConvertToDto(i));
                    });

                }

                catch (Exception ex)
                {
                    message = "***ERROR Role CONTROLLER" +
                       "\nIN CLASS: Role" +
                       "\nWITH METHOD: DeleteRole" +
                       "\nERROR MESSAGE: " + ex.Message;
                    Notification.PostMessage(message);
                    return BadRequest(message);
                }
                return returnObject;
            }
            return NotFound("Cannot find Project to delete");
        }

        public async Task<List<RoleDto>> DeleteRoleRangeInternal(List<RoleDto> rangeToDelete)
        {
            var result = await DeleteRoleRange(rangeToDelete);

            DataConverter<List<RoleDto>> dc2 = new DataConverter<List<RoleDto>>();
            List<RoleDto> returnResult = dc2.GetOkObjectResult(result);


            return returnResult; ;
        }
        // DELETE: api/Roles/deleteRange
        [HttpDelete("deleteRange")]
        public async Task<ActionResult<List<RoleDto>>> DeleteRoleRange([FromBody] List<RoleDto> range)
        {

            try
            {
                List<RoleDto> itemsDeletedDto = new List<RoleDto>();
                List<Role> itemsToDelete = new List<Role>();
                List<Role> deleted = new List<Role>();
                range.ForEach(async item =>
                {

                    Role itemToDelete = await repository.Read(item.ID);
                    try
                    {
                        if (itemToDelete.ID != null)
                        {
                            itemsToDelete.Add(itemToDelete);
                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        message = itemToDelete.ID + " does not exist" + "\n" + ex.Message;
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
                message = "***ERROR Role CONTROLLER" +
                   "\nIN CLASS: Role" +
                   "\nWITH METHOD: DeleteRoleRange" +
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
