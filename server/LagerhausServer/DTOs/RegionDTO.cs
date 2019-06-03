using System.ComponentModel.DataAnnotations;
using LagerhausDb;

namespace Lagerhaus.DTOs
{
    public class RegionDTO
    {
        public string Name { get; set; }
        public string Area { get; set; } = null;
        public int? Level { get; set; } = null;

        public RegionDTO()
        {
        }

        public RegionDTO(Region region)
        {
            this.Name = region.Name;
            this.Area = region.Area;
            this.Level = region.Level;
        }

        public override string ToString() => $"{Name} ({Area}, {Level})";
    }
}
