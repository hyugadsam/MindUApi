using DBService.Entities;
using DBService.Models;
using Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBService.Interfaces
{
    public interface IUserService
    {
        Task<BasicResponse> CreateUser(Users user);
        Task<BasicResponse> UpdateUser(Users user);
        Task<BasicResponse> DeactivateUser(string email);
        Task<LoginResponse> IdentifyUser(string Email, string Password);
        Task<List<Users>> GetList();

    }
}
