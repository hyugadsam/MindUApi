using AutoMapper;
using DBService.Entities;
using Dtos.Dtos;
using Dtos.Request;
using Dtos.Responses;


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
            CreateMap<NewBasicUserRequest, NewUserRequest>();
            CreateMap<NewUserRequest, Users>();
            CreateMap<UpdateUserRequest, Users>();
            CreateMap<DBService.Models.LoginResponse, UserValidationResponse>();
            #endregion

            #region CollaboratorsMaps

            #endregion

        }
    }
}
