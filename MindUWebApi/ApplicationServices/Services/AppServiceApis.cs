using Dtos.Interfaces;
using Dtos.Models;
using Dtos.Request;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Dtos.Responses;
using System.Linq;
using AutoMapper;

namespace ApplicationServices.Services
{
    public class AppServiceApis
    {
        private readonly IWeatherService weatherService;
        private readonly IMapService mapService;
        private readonly ILogger<AppServiceApis> logger;
        private readonly IMapper mapper;

        public AppServiceApis(IWeatherService weatherService, IMapService mapService, ILogger<AppServiceApis> logger, IMapper mapper)
        {
            this.weatherService = weatherService;
            this.mapService = mapService;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<WeatherInfoDto> GetWather(GetWeatherRequest request)
        {
            try
            {
                var response = await weatherService.GetCurrentWeather(request);
                return mapper.Map<WeatherInfoDto>(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return new WeatherInfoDto();
            }
        }

        public async Task<MapBoxResponse> SearchCities(GetCitiesRequest request)
        {
            try
            {
                return await mapService.SearchCities(request);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return new MapBoxResponse();
            }
        }

        public async Task<List<CityInfoDto>> SearchCitiesMin(GetCitiesRequest request)
        {
            try
            {
                var response = await mapService.SearchCities(request);
                return mapper.Map<List<CityInfoDto>>(response.features);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return new List<CityInfoDto>();
            }
        }

        public async Task<CityWithWeather> GetCityAndWeather(string Place)
        {
            try
            {
                var city = mapper.Map<CityInfoDto>(await mapService.SearchCities(new GetCitiesRequest { Name = Place } ));
                var wRequest = new GetWeatherRequest { Latitude = city.Latitude, Longitude = city.Longitude };
                var w = mapper.Map<WeatherInfoDto>(await weatherService.GetCurrentWeather(wRequest));
                return new CityWithWeather()
                {
                    Currtemp = w.Currtemp,
                    Longitude = city.Longitude,
                    Latitude = city.Latitude,
                    Maxtemp = w.Maxtemp,
                    Mintemp = w.Mintemp,
                    Name = city.Name,
                    Weather = w.Weather
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return new CityWithWeather();
            }

        }



    }
}
