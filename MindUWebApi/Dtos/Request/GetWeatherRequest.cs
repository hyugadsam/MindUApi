using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Request
{
    public class GetWeatherRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
