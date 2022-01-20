using ApplicationServices.Services;
using DBService.Models;
using Dtos.Dtos;
using Dtos.Enums;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MindUWebApi.Controllers
{
    [ApiController]
    [Route("api/Users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppServiceUsers service;
        private readonly IConfiguration configuration;

        public UsersController(AppServiceUsers service, IConfiguration configuration )
        {
            this.service = service;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("CreateUser")]
        [Authorize(Policy = "SuperAdminPolicity")]
        public async Task<ActionResult<BasicResponse>> NewUser(NewUserRequest request)
        {
            var result = await service.CreateUser(request);
            return StatusCode(result.Code, result);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicity")]
        [Route("CreateNormalUser")]
        public async Task<ActionResult<BasicResponse>> NewBasicUser(NewBasicUserRequest request)
        {
            var result = await service.CreateBasicUser(request);
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
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<Dtos.Responses.LoginResponse>> IdentifyUser(LoginRequest credentials)
        {
            var result = await service.IdentifyUser(credentials);
            if (result.Code != 200)
            {
                return StatusCode(result.Code, new Dtos.Responses.LoginResponse { Code = result.Code, Message= result.Message });
            }
            else
                return Ok(CreateToken(result));
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var result = await service.GetAllUsers();
            return Ok(result);
        }

        private Dtos.Responses.LoginResponse CreateToken(UserValidationResponse user)
        {
            var rol = ((EnumRoles)user.User.RoleId).ToString();
            var claims = new List<Claim>()
            {
                new Claim("Name", user.User.Name),
                new Claim("Email", user.User.Email),
                new Claim("Role", rol)
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

            return new Dtos.Responses.LoginResponse()
            {
                Code = 200,
                Expriation = expiration,
                Message = user.Message,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
                
            };
        }


    }
}
