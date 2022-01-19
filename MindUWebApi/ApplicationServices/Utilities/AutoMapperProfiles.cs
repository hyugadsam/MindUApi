using AutoMapper;
using DBService.Entities;
using Dtos.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationServices.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Roles, RoleDto>();
        }
    }
}
