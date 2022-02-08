using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Responses
{
    public class WeatherInfoDto
    {
        public string Weather { get; set; }
        public string Currtemp { get; set; }
        public string Maxtemp { get; set; }
        public string Mintemp { get; set; }

    }
}
