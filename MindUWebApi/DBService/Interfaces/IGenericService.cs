using Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBService.Interfaces
{
    public interface IGenericService<T>
    {
        Task<BasicResponse> Create(T obj);
        Task<BasicResponse> Update(T obj);
        Task<BasicResponse> Delete(int Id);
        Task<List<T>> GetList();

    }
}
