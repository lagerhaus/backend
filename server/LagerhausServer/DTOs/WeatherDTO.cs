using LagerhausDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagerhausServer.DTOs
{
    public class WeatherDTO
    {
        public String Region { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? RainyDays { get; set; }
        public int? SunnyDays { get; set; }


        public WeatherDTO(Weather w)
        {
            this.Region = w.Region.Name;
            this.Year = w.Year;
            this.Month = w.Month;
            this.RainyDays = w.RainyDays;
            this.SunnyDays = w.SunnyDays;           
        }
    }
}
