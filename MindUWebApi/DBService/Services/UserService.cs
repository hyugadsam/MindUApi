using DBService.Entities;
using DBService.Interfaces;
using DBService.Models;
using DBService.Security;
using Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBService.Services
{
    public class UserService : IUserService
    {
        private readonly MindUContext context;
        private readonly ILogger<UserService> logger;

        public UserService(MindUContext context, ILogger<UserService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<BasicResponse> CreateUser(Users user)
        {
            try
            {
                var exist = await context.Users.AnyAsync(u => u.Email == user.Email);
                if (exist) return new BasicResponse() { Message = "User al ready exist", Code = 400 };

                user.Salt = Hash.GetNewSalt();  //Genera la cadena de encryptado
                user.Password = Hash.HashPasword(user.Password, user.Salt); //Encrypta el password
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return new BasicResponse()
                {
                    Message = "Created Success",
                    Code = 200
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"Error at DBService.Services.UserService.CreateUser: \n {ex.Message}\nStackTrace:{ex.StackTrace}");
                return new BasicResponse()
                {
                    Message = "Creation Error Verify the request",
                    Code = 500
                };
            }
            
        }

        public async Task<BasicResponse> DeactivateUser(string email)
        {
            try
            {
                var user = await context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();

                if (user == null) return new BasicResponse(){ Message = "User dosent exist", Code = 400 };

                user.IsActive = false;
                context.Entry(user).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return new BasicResponse()
                {
                    Message = "Deactivated Success",
                    Code = 200
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"Error at DBService.Services.UserService.DeactivateUser: \n {ex.Message}\nStackTrace:{ex.StackTrace}");
                return new BasicResponse()
                {
                    Message = "Creation Error Verify the request",
                    Code = 500
                };
            }
        }

        public async Task<LoginResponse> IdentifyUser(string Email, string Password)
        {
            try
            {
                var userDb = await ExistUser(Email);
                if (userDb == null) return new LoginResponse() { Message = "User dosent exist", Code = 400 };

                if (userDb.Password.Equals(Hash.HashPasword(Password, userDb.Salt)))
                    return new LoginResponse() { Code = 200, Message = "User identified success", User = userDb };
                else
                    return new LoginResponse() { Code = 400, Message = "Invalid credentials" }; //Wrong password

            }
            catch (Exception ex)
            {
                logger.LogError($"Error at DBService.Services.UserService.IdentifyUser: \n {ex.Message}\nStackTrace:{ex.StackTrace}");
                return new LoginResponse() { Code = 500, Message = "Internal Error, contact support" };
            }
        }

        public async Task<BasicResponse> UpdateUser(Users user)
        {
            try
            {
                var userDb = await ExistUser(user.Email);

                if (userDb == null) return new BasicResponse() { Message = "User dosent exist", Code = 400 };

                userDb.IsActive = user.IsActive;
                userDb.Name = user.Name;
                userDb.RoleId = user.RoleId;
                if (!string.IsNullOrEmpty(user.Password))//Si envia el pass, se actualiza
                {
                    userDb.Salt = Hash.GetNewSalt();  //Genera la cadena de encryptado
                    userDb.Password = Hash.HashPasword(user.Password, userDb.Salt); //Encrypta el password
                }
                context.Entry(userDb).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return new BasicResponse()
                {
                    Message = "Update Success",
                    Code = 200
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"Error at DBService.Services.UserService.UpdateUser: \n {ex.Message}\nStackTrace:{ex.StackTrace}");
                return new BasicResponse()
                {
                    Message = "Creation Error Verify the request",
                    Code = 500
                };
            }

        }

        public async Task<List<Users>> GetList()
        {
            try
            {
                throw new Exception("Test");
                logger.LogInformation("Test Log");
                return await context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Message", 1);
                //logger.LogError($"Error at DBService.Services.UserService.GetList: \n {ex.Message}\nStackTrace:{ex.StackTrace}");
                return null;
            }
        }

        private async Task<Users> ExistUser(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }


    }
}
