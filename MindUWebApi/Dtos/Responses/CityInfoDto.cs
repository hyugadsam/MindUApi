using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Responses
{
    public class CityInfoDto
    {
        public string PlaceId { get; set; }
        public string Name { get; set; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }

    }
}
