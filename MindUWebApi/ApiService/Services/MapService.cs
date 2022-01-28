using Dtos.Interfaces;
using Dtos.Models;
using Dtos.Request;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiService.Services
{
    public class MapService : IMapService
    {
        private readonly HttpClient client;
        private readonly string Token;

        public MapService(HttpClient client, IConfiguration configuration)
        {
            this.client = client;
            Token = configuration["TokenMapBox"];

        }

        public async Task<MapBoxResponse> SearchCities(GetCitiesRequest request)
        {
            var resp = await client.GetAsync($"v5/mapbox.places/{request.Name}.json?limit=5&language=es&access_token={Token}");

            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                return new MapBoxResponse { features = new List<Feature>() };


            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MapBoxResponse>(response);

        }




    }
}
