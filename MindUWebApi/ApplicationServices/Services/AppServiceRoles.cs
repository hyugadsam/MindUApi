using AutoMapper;
using DBService.Entities;
using DBService.Interfaces;
using DBService.Services;
using Dtos.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Services
{
    public class AppServiceRoles
    {
        //private readonly MindUContext context;
        private readonly IMapper mapper;
        private readonly IRoleService rolService;

        public AppServiceRoles(MindUContext context, IMapper mapper, ILogger<RolesService> logger)
        {
            this.mapper = mapper;
            rolService = new RolesService(context, logger);
        }

        public async Task<List<RoleDto>> GetRoles()
        {
            var roles = await rolService.GetRoles();
            return mapper.Map<List<RoleDto>>(roles);
        }


    }
}
