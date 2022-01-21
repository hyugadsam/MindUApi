using ApplicationServices.Services;
using Dtos.Dtos;
using Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindUWebApi.Controllers
{
    [ApiController]
    [Route("api/Catalogs/Levels")]
    public class LevelsController : ControllerBase
    {
        private readonly AppServiceLevels service;

        public LevelsController(AppServiceLevels service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<BasicResponse>> CreateTechnology([FromBody] string description)
        {
            var result = await service.CreateLevel(description);
            return StatusCode(result.Code, result);
        }
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<BasicResponse>> UpdateTechnology(LevelDto obj)
        {
            var result = await service.UpdateLevel(obj);
            return StatusCode(result.Code, result);
        }
        [HttpDelete]
        [Route("Delete/{LevelId:int}")]
        public async Task<ActionResult<BasicResponse>> Delete(int LevelId)
        {
            var result = await service.Delete(LevelId);
            return StatusCode(result.Code, result);
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<LevelDto>>> GetList()
        {
            return await service.GetList();
        }


    }
}
