using AutoMapper;
using DBService.Entities;
using DBService.Interfaces;
using DBService.Services;
using Dtos.Dtos;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationServices.Services
{
    public class AppServiceUsers
    {
        private readonly IUserService service;
        private readonly IMapper mapper;

        public AppServiceUsers(ILogger<UserService> logger, MindUContext context, IMapper mapper)
        {
            service = new UserService(context, logger);
            this.mapper = mapper;
        }

        public async Task<BasicResponse> CreateUser(NewUserRequest user)
        {
            var userDb = mapper.Map<Users>(user);
            return await service.CreateUser(userDb);
        }

        public async Task<BasicResponse> UpdateUser(UpdateUserRequest user)
        {
            var userDb = mapper.Map<Users>(user);
            return await service.UpdateUser(userDb);
        }

        public async Task<BasicResponse> DeactivateUser(string email)
        {
            return await service.DeactivateUser(email);
        }

        public async Task<UserValidationResponse> IdentifyUser(string Email, string Password)
        {
            var user = await service.IdentifyUser(Email, Password);
            return mapper.Map<UserValidationResponse>(user);
        }
        
        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await service.GetList();
            return mapper.Map<List<UserDto>>(users);
        }



    }

}
