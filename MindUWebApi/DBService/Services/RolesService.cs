using DBService.Entities;
using DBService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBService.Services
{
    public class RolesService : IRoleService
    {
        private readonly MindUContext context;
        private readonly ILogger<RolesService> logger;

        public RolesService(MindUContext context, ILogger<RolesService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<List<Roles>> GetRoles()
        {
            try
            {
                return await context.Roles.Where(r => r.RoleId > 1).ToListAsync();  //No enviar el SuperAdmin
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error at DBService.Services.RolesService.GetRoles");
                return null;
            }
            
        }

    }
}
