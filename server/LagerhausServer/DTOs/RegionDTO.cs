using LagerhausDb;

namespace Lagerhaus.DTOs
{
    public class RegionDTO
    {
        public string Name { get; set; }
        public string Area { get; set; }
        public int? Level { get; set; }

        public RegionDTO(Region region)
        {
            this.Name = region.Name;
            this.Area = region.Area;
            this.Level = region.Level;
        }
    }
}
