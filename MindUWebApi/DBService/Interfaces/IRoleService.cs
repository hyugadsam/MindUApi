using DBService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Interfaces
{
    public interface IRoleService
    {
        Task<List<Roles>> GetRoles();

    }
}
