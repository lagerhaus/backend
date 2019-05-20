using System;
using System.Collections.Generic;

namespace LagerhausDb
{
    public partial class Fruit
    {
        public Fruit()
        {
            Batch = new HashSet<Batch>();
            Ripeness = new HashSet<Ripeness>();
        }

        public int FruitId { get; set; }
        public string Name { get; set; }

        public ICollection<Batch> Batch { get; set; }
        public ICollection<Ripeness> Ripeness { get; set; }
    }
}
