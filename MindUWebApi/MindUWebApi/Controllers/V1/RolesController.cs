using ApplicationServices.Services;
using Dtos.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
//using System.Security.Claims;

namespace MindUWebApi.Controllers.V1
{
    [ApiController]
    [Route("api/v1/Roles")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly AppServiceRoles appService;

        public RolesController(AppServiceRoles appService)
        {
            this.appService = appService;
        }

        [HttpGet]
        [Route("GetRolesSuperAdminLevel")]
        [Authorize(Policy = "SuperAdminPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<RoleDto>>> Get()
        {
            return await appService.GetRoles();
        }

        [HttpGet]
        [Route("GetRolesAdminLevel")]
        [Authorize(Policy = "AdminPolicity")]
        public async Task<ActionResult<List<RoleDto>>> Get2()
        {
            return await appService.GetRoles();
        }


        [HttpGet]
        [Route("GetRolesUserLevel")]
        [Authorize(Policy = "UserPolicity")]
        public async Task<ActionResult<List<RoleDto>>> Get3()
        {
            //Get the token info
            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            //IEnumerable<Claim> claims = identity.Claims
            return await appService.GetRoles();
        }

        [HttpGet]
        [Route("GetRolesPublic")]
        [AllowAnonymous]
        public async Task<ActionResult<List<RoleDto>>> Get4()
        {
            return await appService.GetRoles();
        }

    }
}
