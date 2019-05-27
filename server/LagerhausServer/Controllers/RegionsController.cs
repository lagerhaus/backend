using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lagerhaus.DTOs;
using LagerhausDb;

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
    }
}
