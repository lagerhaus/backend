using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LagerhausDb
{
    public partial class Region
    {
        public Region()
        {
            Batch = new HashSet<Batch>();
            Weather = new HashSet<Weather>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegionId { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public int? Level { get; set; }

        public ICollection<Batch> Batch { get; set; }
        public ICollection<Weather> Weather { get; set; }
    }
}
