using LagerhausDb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagerhausServer.DTOs
{
    public class FruitDTO
    {
        public FruitDTO()
        {

        }

        public FruitDTO(Fruit f)
        {
            Name = f.Name;
            Ripeness = f.Ripeness.Select(x =>new RipenessDTO(x)).ToList();
        }

        public String Name { get; set; }
        [JsonProperty("ripeness_grades")]
        public ICollection<RipenessDTO> Ripeness { get; set; }
    }
}
