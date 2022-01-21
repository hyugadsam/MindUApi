using ApplicationServices.Services;
using Dtos.Dtos;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindUWebApi.Controllers
{
    [ApiController]
    [Route("api/Collaborators")]
    public class CollaboratorsController : ControllerBase
    {
        private readonly AppServiceCollaborators service;

        public CollaboratorsController(AppServiceCollaborators service)
        {
            this.service = service;
        }

        [Route("Create")]
        [HttpPost]
        public async Task<ActionResult<BasicResponse>> Create(NewCollaboratorRequest obj)
        {
            var data = await service.Create(obj);
            return StatusCode(data.Code, data);
        }

        [Route("Delete")]
        [HttpDelete]
        public async Task<ActionResult<BasicResponse>> Delete(int CollaboratorId)
        {
            var data = await service.Delete(CollaboratorId);
            return StatusCode(data.Code, data);
        }

        [Route("Get")]
        [HttpGet]
        public async Task<ActionResult<List<CollaboratorDto>>> GetList()
        {
            return await service.GetList();
        }

        [Route("Get/{CollaboratorId:int}")]
        [HttpGet]
        public async Task<ActionResult<CollaboratorFullDto>> Get(int CollaboratorId)
        {
            return await service.Get(CollaboratorId);
        }

        [Route("Update")]
        [HttpPut]
        public async Task<ActionResult<BasicResponse>> Update(UpdateCollaboratorRequest obj)
        {
            var data = await service.Update(obj);
            return StatusCode(data.Code, data);
        }


    }
}
