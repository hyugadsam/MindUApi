using Dtos.Models;
using Dtos.Request;
using System.Threading.Tasks;

namespace Dtos.Interfaces
{
    public interface IWeatherService
    {
        Task<OpenWatherResponse> GetCurrentWeather(GetWeatherRequest request);
    }
}
