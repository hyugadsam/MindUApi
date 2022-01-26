using DBService.Entities;
using Dtos.Request;
using Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBService.Interfaces
{
    public interface IUserService
    {
        Task<BasicCreateResponse> CreateUser(Users user);
        Task<BasicResponse> UpdateUser(Users user);
        Task<BasicResponse> DeactivateUser(string email);
        Task<Models.LoginResponse> IdentifyUser(LoginRequest credentials);
        Task<List<Users>> GetList();

    }
}
