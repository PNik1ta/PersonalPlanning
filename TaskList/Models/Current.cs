using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    public class Current
    {
        public string ObservationTime { get; set; }
        public double Temperature { get; set; }
        public double WeatherCode { get; set; }
        public string WeatherIcons { get; set; }
        public string WeatherDescriptions { get; set; }

        [JsonProperty("wind_speed")]
        public double WindSpeed { get; set; }
        public double WindDegree { get; set; }
        public string WindDir { get; set; }
        public double Pressure { get; set; }
        public double Precip { get; set; }
        public double Humidity { get; set; }
        public double CloudCover { get; set; }
        public double FeelsLike { get; set; }
        public int Uv_Index { get; set; }
        public double Visibility { get; set; }
        public string IsDay { get; set; }
    }
}
