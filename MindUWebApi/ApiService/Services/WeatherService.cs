using Dtos.Models;
using Dtos.Request;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using Dtos.Interfaces;
using System.Threading.Tasks;

namespace ApiService.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient client;
        private readonly string Token;

        public WeatherService(HttpClient client, IConfiguration configuration)
        {
            this.client = client;
            Token = configuration["TokenWeather"];

        }

        public async Task<OpenWatherResponse> GetCurrentWeather(GetWeatherRequest request)
        {
            var resp = await client.GetAsync($"data/2.5/onecall?lat={request.Latitude}&lon={request.Longitude}&appid={Token}&dt={DateTime.Now.Ticks}&lang=es");

            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                return new OpenWatherResponse { timezone = string.Empty };


            var response = await resp.Content.ReadAsStringAsync();
            var x = JsonConvert.DeserializeObject<OpenWatherResponse>(response);
            return x;

        }




    }
}
