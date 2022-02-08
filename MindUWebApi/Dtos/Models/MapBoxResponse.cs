using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Models
{
    public class MapBoxResponse
    {
        public List<Feature> features { get; set; }
    }

    public class Feature
    {
        public string id { get; set; }
        public string text_es { get; set; } //Nombre Ciudad
        public string place_name { get; set; } //Lugar completo
        public string place_name_es { get; set; } //Lugar completo
        public Geometry geometry { get; set; }

    }
    public class Geometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }


}
