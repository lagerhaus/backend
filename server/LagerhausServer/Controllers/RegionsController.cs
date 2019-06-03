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
using Lagerhaus.Processors;

namespace LagerhausServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private RegionsValidation validation;
        private RegionsProcessor processor;

        public RegionsController(RegionsValidation validation, RegionsProcessor processor)
        {
            this.validation = validation;
            this.processor = processor;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RegionDTO>> GetAllRegions()
        {
            return this.processor.GetAllRegions()
                .Select(r => new RegionDTO(r))
                .ToList();
        }

        [HttpGet("{regionName}")]
        public ActionResult<RegionDTO> GetRegion([FromRoute] string regionName)
        {
            var region = this.processor.GetSingleRegion(regionName);

            if (region == null)
                return BadRequest(new NoSuchResourceError("No region with this name found"));

            return new RegionDTO(region);
        }

        [HttpPost]
        public ActionResult<RegionDTO> PostRegion([FromBody] RegionDTO dto)
        {
            System.Console.WriteLine($"RegionsController::PostRegion({dto})");

            var validationError = this.validation.ValidateRegionCreationDTO(dto);
            if (validationError != null)
                return BadRequest(validationError);

            try
            {
                var region = this.processor.InsertRegion(dto);
                return new RegionDTO(region);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new DuplicateKeyError("A region with this name already exists"));
            }
        }
    }
}
