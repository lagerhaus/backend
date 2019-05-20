using System;
using System.Collections.Generic;

namespace LagerhausDb
{
    public partial class Region
    {
        public Region()
        {
            Batch = new HashSet<Batch>();
            Weather = new HashSet<Weather>();
        }

        public int RegionId { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public int? Level { get; set; }

        public ICollection<Batch> Batch { get; set; }
        public ICollection<Weather> Weather { get; set; }
    }
}
