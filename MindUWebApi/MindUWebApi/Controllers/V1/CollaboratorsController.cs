using ApplicationServices.Services;
using Dtos.Dtos;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindUWebApi.Controllers.V1
{
    [ApiController]
    [Route("api/v1/Collaborators")]
    public class CollaboratorsController : ControllerBase
    {
        private readonly AppServiceCollaborators service;

        public CollaboratorsController(AppServiceCollaborators service)
        {
            this.service = service;
        }

        [Route("Create")]
        [Authorize(Policy = "AdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<BasicCreateResponse>> Create(NewCollaboratorRequest obj)
        {
            var data = await service.Create(obj);
            if (data.Code != 200)
            {
                return StatusCode(data.Code, data);
            }
            return data;
        }

        [Route("Delete")]
        [Authorize(Policy = "SuperAdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        public async Task<ActionResult<BasicResponse>> Delete(int CollaboratorId)
        {
            var data = await service.Delete(CollaboratorId);
            if (data.Code != 200)
            {
                return StatusCode(data.Code, data);
            }
            return data;
        }

        [Route("Get")]
        [HttpGet]
        [Authorize(Policy = "AdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<CollaboratorDto>>> GetList()
        {
            return await service.GetList();
        }

        [Route("Get/{CollaboratorId:int}")]
        [HttpGet]
        [Authorize(Policy = "AdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CollaboratorFullDto>> Get(int CollaboratorId)
        {
            return await service.Get(CollaboratorId);
        }

        [Route("Update")]
        [HttpPut]
        [Authorize(Policy = "AdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<BasicResponse>> Update(UpdateCollaboratorRequest obj)
        {
            var data = await service.Update(obj);
            if (data.Code != 200)
            {
                return StatusCode(data.Code, data);
            }
            return data;
        }

        [HttpPut]
        [Route("Deactivate/{CollaboratorId:int}")]
        [Authorize(Policy = "AdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<BasicResponse>> DeactivateCollaborator(int CollaboratorId)
        {
            var data = await service.DeactivateCollaborator(CollaboratorId);
            if (data.Code != 200)
            {
                return StatusCode(data.Code, data);
            }
            return data;
        }

        [HttpPut]
        [Route("Graduate/{CollaboratorId:int}")]
        [Authorize(Policy = "AdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<BasicResponse>> GraduateCollaborator(int CollaboratorId)
        {
            var data = await service.GraduateCollaborator(CollaboratorId);
            if (data.Code != 200)
            {
                return StatusCode(data.Code, data);
            }
            return data;
        }


    }
}
