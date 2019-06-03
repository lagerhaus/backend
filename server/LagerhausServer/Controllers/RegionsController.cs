using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lagerhaus.DTOs;
using LagerhausDb;
using Lagerhaus.Errors;

namespace LagerhausServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private LagerhausContext db;

        public RegionsController(LagerhausContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RegionDTO>> GetAllRegions()
        {
            return this.db.Region
                .Select(r => new RegionDTO(r))
                .ToList();
        }

        [HttpGet("{regionName}")]
        public ActionResult<RegionDTO> GetRegion([FromRoute] string regionName)
        {
            var region = this.db.Region
                .Where(r => r.Name == regionName)
                .Select(r => new RegionDTO(r))
                .SingleOrDefault();

            if (region == null)
                return BadRequest(new NoSuchResourceError("No region with this name found"));

            return region;
        }
    }
}
