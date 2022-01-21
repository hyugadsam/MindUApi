using ApplicationServices.Services;
using Dtos.Dtos;
using Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindUWebApi.Controllers
{
    [ApiController]
    [Route("api/Catalogs/Technologies")]
    public class TechnologiesController : ControllerBase
    {
        private readonly AppServiceTechnologies service;

        public TechnologiesController(AppServiceTechnologies service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<BasicResponse>> CreateTechnology([FromBody] string description)
        {
            var result = await service.CreateTechnology(description);
            return StatusCode(result.Code, result);
        }
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<BasicResponse>> UpdateTechnology(TechnologyDto obj)
        {
            var result = await service.UpdateTechnology(obj);
            return StatusCode(result.Code, result);
        }
        [HttpDelete]
        [Route("Delete/{TechnologyId:int}")]
        public async Task<ActionResult<BasicResponse>> Delete(int TechnologyId)
        {
            var result = await service.Delete(TechnologyId);
            return StatusCode(result.Code, result);
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<TechnologyDto>>> GetList()
        {
            return await service.GetList();
        }


    }
}
