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
    [Route("api/v1/Catalogs/Levels")]
    public class LevelsController : ControllerBase
    {
        private readonly AppServiceLevels service;

        public LevelsController(AppServiceLevels service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Policy = "AdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<BasicCreateResponse>> Create([FromBody] string description)
        {
            var result = await service.CreateLevel(description);
            if (result.Code != 200)
            {
                return StatusCode(result.Code, result);
            }
            return result;
        }
        [HttpPut]
        [Route("Update")]
        [Authorize(Policy = "AdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<BasicResponse>> Update(LevelDto obj)
        {
            var result = await service.UpdateLevel(obj);
            if (result.Code != 200)
            {
                return StatusCode(result.Code, result);
            }
            return result;
        }
        [HttpDelete]
        [Route("Delete/{LevelId:int}")]
        [Authorize(Policy = "SuperAdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<BasicResponse>> Delete(int LevelId)
        {
            var result = await service.Delete(LevelId);
            if (result.Code != 200)
            {
                return StatusCode(result.Code, result);
            }
            return result;
        }
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Policy = "AdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<LevelDto>>> GetList()
        {
            return await service.GetList();
        }


    }
}
