using ApplicationServices.Services;
using DBService.Models;
using Dtos.Dtos;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindUWebApi.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UsersController : ControllerBase
    {
        private readonly AppServiceUsers service;

        public UsersController(AppServiceUsers service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<ActionResult<BasicResponse>> NewUser(NewUserRequest request)
        {
            var result = await service.CreateUser(request);
            return StatusCode(result.Code, result);
        }
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<ActionResult<BasicResponse>> UpdateUser(UpdateUserRequest user)
        {
            var result = await service.UpdateUser(user);
            return StatusCode(result.Code, result);
        }

        [HttpPut]
        [Route("Deactivate")]
        public async Task<ActionResult<BasicResponse>> DeactivateUser(string Email)
        {
            var result = await service.DeactivateUser(Email);
            return StatusCode(result.Code, result);
        }

        [HttpPost]
        [Route("Identify")]
        public async Task<ActionResult<UserValidationResponse>> IdentifyUser(string Email, string Password)
        {
            var result = await service.IdentifyUser(Email, Password);
            return StatusCode(result.Code, result);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var result = await service.GetAllUsers();
            return Ok(result);
        }

    }
}
