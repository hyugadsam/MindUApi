using ApplicationServices.Services;
using Dtos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindUWebApi.Controllers
{
    [ApiController]
    [Route("api/Roles")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly AppServiceRoles appService;

        public RolesController(AppServiceRoles appService)
        {
            this.appService = appService;
        }

        [HttpGet]
        [Route("GetRolesSuperAdmin")]
        [Authorize(Policy = "SuperAdminPolicity")]
        public async Task<ActionResult<List<RoleDto>>> Get()
        {
            return Ok(await appService.GetRoles());
        }

        [HttpGet]
        [Route("GetRolesAdmin")]
        [Authorize(Policy = "AdminPolicity")]
        public async Task<ActionResult<List<RoleDto>>> Get2()
        {
            return Ok(await appService.GetRoles());
        }


        [HttpGet]
        [Route("GetRolesNone")]
        [Authorize(Policy = "UserPolicity")]
        public async Task<ActionResult<List<RoleDto>>> Get4()
        {
            return Ok(await appService.GetRoles());
        }

    }
}
