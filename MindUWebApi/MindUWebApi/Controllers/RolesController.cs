using ApplicationServices.Services;
using Dtos.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<List<RoleDto>> Get()
        {
            return await appService.GetRoles();
        }




    }
}
