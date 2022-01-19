﻿using DBService.Entities;
using DBService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBService.Services
{
    public class RolesService : IRoleService
    {
        private readonly MindUContext context;

        public RolesService(MindUContext context)
        {
            this.context = context;
        }

        public async Task<List<Roles>> GetRoles()
        {
            return await context.Roles.ToListAsync();
        }

    }
}
