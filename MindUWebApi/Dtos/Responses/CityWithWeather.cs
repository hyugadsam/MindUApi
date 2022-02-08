using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Responses
{
    public class CityWithWeather : WeatherInfoDto
    {
        public string Name { get; set; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }
    }


}
