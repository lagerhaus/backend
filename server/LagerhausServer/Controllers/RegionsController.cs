using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lagerhaus.DTOs;
using LagerhausDb;
using Lagerhaus.Errors;
using Lagerhaus.Validation;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace LagerhausServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private LagerhausContext db;
        private RegionsValidation validation;

        public RegionsController(LagerhausContext db, RegionsValidation validation)
        {
            this.db = db;
            this.validation = validation;
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

        [HttpPost]
        public ActionResult<RegionDTO> PostRegion([FromBody] RegionDTO dto)
        {
            System.Console.WriteLine($"RegionsController::PostRegion({dto})");

            var validationError = this.validation.ValidateRegionDTO(dto);
            if (validationError != null)
                return BadRequest(validationError);

            var region = new Region
            {
                Name = dto.Name,
                Area = dto.Area,
                Level = dto.Level
            };

            try
            {
                this.db.Region.Add(region);
                this.db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new DuplicateKeyError("A region with this name already exists"));
            }

            return new RegionDTO(region);
        }
    }
}
