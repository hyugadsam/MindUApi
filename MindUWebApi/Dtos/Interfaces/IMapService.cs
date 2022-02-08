using Dtos.Models;
using Dtos.Request;
using System.Threading.Tasks;

namespace Dtos.Interfaces
{
    public interface IMapService
    {
        Task<MapBoxResponse> SearchCities(GetCitiesRequest request);
    }
}
