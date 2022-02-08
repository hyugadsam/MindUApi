using ApplicationServices.Services;
using Dtos.Models;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace MindUWebApi.Controllers.V2
{
    [ApiController]
    [Route("api/ExternalServices")]
    //[Authorize(Policy = "UserPolicity", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExternalServicesController : ControllerBase
    {
        private readonly AppServiceApis Service;

        public ExternalServicesController(AppServiceApis appService)
        {
            Service = appService;
        }

        [HttpPost]
        [Route("GetWather")]
        public async Task<WeatherInfoDto> GetWather(GetWeatherRequest request)
        {
            return await Service.GetWather(request);
        }

        [HttpPost]
        [Route("SearchCities")]
        public async Task<MapBoxResponse> SearchCities(GetCitiesRequest request)
        {
            return await Service.SearchCities(request);
        }

        [HttpPost]
        [Route("SearchCitiesMin")]
        public async Task<List<CityInfoDto>> SearchCitiesMin(GetCitiesRequest request)
        {
            return await Service.SearchCitiesMin(request);
        }

        [HttpGet]
        [Route("GetCityAndWeather")]
        public async Task<CityWithWeather> GetCityAndWeather(string Place)
        {
            return await Service.GetCityAndWeather(Place);
        }


    }
}
