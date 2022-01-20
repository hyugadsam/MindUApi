using AutoMapper;
using DBService.Entities;
using DBService.Models;
using Dtos.Dtos;
using Dtos.Request;
using Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationServices.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            #region RolesMaps
            CreateMap<Roles, RoleDto>();
            #endregion

            #region UserMaps
            CreateMap<Users, UserDto>();
            CreateMap<NewUserRequest, Users>();
            CreateMap<UpdateUserRequest, Users>();
            CreateMap<LoginResponse, UserValidationResponse>();
            #endregion

            #region CollaboratorsMaps

            #endregion

        }
    }
}
