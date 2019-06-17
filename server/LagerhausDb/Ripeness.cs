using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LagerhausDb
{
    public partial class Ripeness
    {
        public Ripeness()
        {
            Batch = new HashSet<Batch>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RipenessId { get; set; }
        public string Name { get; set; }
        public int? FruitId { get; set; }
        public int? MinimumStorageSpan { get; set; }

        public Fruit Fruit { get; set; }
        public ICollection<Batch> Batch { get; set; }
    }
}
