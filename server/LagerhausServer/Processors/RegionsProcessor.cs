using System.Collections.Generic;
using System.Linq;
using Lagerhaus.DTOs;
using LagerhausDb;

namespace Lagerhaus.Processors
{
    public class RegionsProcessor
    {
        private LagerhausContext db;

        public RegionsProcessor(LagerhausContext db)
        {
            this.db = db;
        }

        public LagerhausContext Db { get => db; set => db = value; }

        public IEnumerable<Region> GetAllRegions() => this.db.Region;

        public Region GetSingleRegion(string regionName) =>
            this.db.Region
                .FirstOrDefault(r => r.Name == regionName);

        public Region InsertRegion(RegionDTO dto)
        {
            var region = new Region
            {
                Name = dto.Name,
                Area = dto.Area,
                Level = dto.Level
            };

            this.db.Region.Add(region);
            this.db.SaveChanges();

            return region;
        }

        public Region UpdateRegion(string regionName, RegionDTO dto)
        {
            var region = this.db.Region
                .First(r => r.Name == regionName);

            if (dto.Name != null)
                region.Name = dto.Name;
            if (dto.Area != null)
                region.Area = dto.Area;
            if (dto.Level != null)
                region.Level = dto.Level;

            this.db.Update(region);
            this.db.SaveChanges();

            return region;
        }
    }
}