using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LagerhausDb
{
    public partial class Fruit
    {
        public Fruit()
        {
            Batch = new HashSet<Batch>();
            Ripeness = new HashSet<Ripeness>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FruitId { get; set; }
        public string Name { get; set; }

        public ICollection<Batch> Batch { get; set; }
        public ICollection<Ripeness> Ripeness { get; set; }
    }
}
