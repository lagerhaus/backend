using System;
using System.Collections.Generic;

namespace LagerhausDb
{
    public partial class Batch
    {
        public int BatchId { get; set; }
        public int? FruitId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Amount { get; set; }
        public DateTime? StorageDate { get; set; }
        public int? RegionId { get; set; }
        public int? RipenessId { get; set; }

        public Fruit Fruit { get; set; }
        public Region Region { get; set; }
        public Ripeness Ripeness { get; set; }
    }
}
