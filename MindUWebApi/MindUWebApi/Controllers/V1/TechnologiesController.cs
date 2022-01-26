using ApplicationServices.Services;
using Dtos.Dtos;
using Dtos.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindUWebApi.Controllers.V1
{
    [ApiController]
    [Route("api/v1/Catalogs/Technologies")]
    public class TechnologiesController : ControllerBase
    {
        private readonly AppServiceTechnologies service;

        public TechnologiesController(AppServiceTechnologies service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Policy = "AdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<BasicCreateResponse>> CreateTechnology([FromBody] string description)
        {
            var result = await service.CreateTechnology(description);
            if (result.Code != 200)
            {
                return StatusCode(result.Code, result);
            }
            return result;
        }
        [HttpPut]
        [Route("Update")]
        [Authorize(Policy = "AdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<BasicResponse>> UpdateTechnology(TechnologyDto obj)
        {
            var result = await service.UpdateTechnology(obj);
            if (result.Code != 200)
            {
                return StatusCode(result.Code, result);
            }
            return result;
        }
        [HttpDelete]
        [Route("Delete/{TechnologyId:int}")]
        [Authorize(Policy = "SuperAdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<BasicResponse>> Delete(int TechnologyId)
        {
            var result = await service.Delete(TechnologyId);
            if (result.Code != 200)
            {
                return StatusCode(result.Code, result);
            }
            return result;
        }
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Policy = "UserPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<TechnologyDto>>> GetList()
        {
            return await service.GetList();
        }


    }
}
