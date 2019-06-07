using LagerhausDb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagerhausServer.DTOs
{
    public class WeatherDTO
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public String Region { get; set; }    
        [JsonProperty ("rainy_days")]
        public int? rainy_days { get; set; }
        [JsonProperty("sunny_days")]
        public int? sunny_days { get; set; }

        public WeatherDTO()
        {

        }

        public WeatherDTO(Weather w)
        {
            this.Region = w.Region.Name;
            this.Year = w.Year;
            this.Month = w.Month;
            this.rainy_days = w.RainyDays;
            this.sunny_days = w.SunnyDays;           
        }

        public Weather ToWeather(LagerhausContext db)
        {
            var w = new Weather
            {
                Year = this.Year,
                Month = this.Month,
                RainyDays = this.rainy_days,
                SunnyDays = this.sunny_days,
                Region = db.Region.Where(x => x.Name.ToLower() == Region.ToLower()).Single(),
                
            };

            return w;
        }
    }
}
