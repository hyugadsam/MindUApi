using ApplicationServices.Services;
using Dtos.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindUWebApi.Controllers
{
    [ApiController]
    [Route("api/Roles")]
    public class RolesController : ControllerBase
    {
        private readonly AppServiceRoles appService;

        public RolesController(AppServiceRoles appService)
        {
            this.appService = appService;
        }

        [HttpGet]
        [ResponseCache(Duration = 9000)] //Para que guarde el response en el cache y no haga tantas consultas al bd
        public async Task<ActionResult<List<RoleDto>>> Get()
        {
            return Ok(await appService.GetRoles());
        }

    }
}
