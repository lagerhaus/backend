using LagerhausDb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagerhausServer.DTOs
{
    public class RipenessDTO
    {
        public RipenessDTO() { }
        public RipenessDTO(Ripeness r)
        {
            Name = r.Name;
            minimumStorageSpan = r.MinimumStorageSpan;
        }
        public string Name { get; set; }

        [JsonProperty("minimum_storage_span")]
        public int? minimumStorageSpan { get; set; }
    }
}
