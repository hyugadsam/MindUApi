using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Models
{
    public class OpenWatherResponse
    {
        public string timezone { get; set; }
        public Current current { get; set; }
        public List<Daily> daily { get; set; }
    }

    public class Current
    {
        public double temp { get; set; }    //Temp actual

        public List<Weather> weather { get; set; }  //Clima
    }

    public class Weather
    {
        public int id { get; set; }
        public string description { get; set; } //Estado clima
    }

    public class Daily
    {
        public Temp temp { get; set; }
    }

    public class Temp
    {
        public double min { get; set; }
        public double max { get; set; }
    }

}
