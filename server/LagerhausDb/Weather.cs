using System;
using System.Collections.Generic;

namespace LagerhausDb
{
    public partial class Weather
    {
        public int WeatherId { get; set; }
        public int? RegionId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? RainyDays { get; set; }
        public int? SunnyDays { get; set; }

        public Region Region { get; set; }
    }
}
