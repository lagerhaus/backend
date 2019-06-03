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
            try
            {
                var region = this.processor.GetSingleRegion(regionName);
                return new RegionDTO(region);
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new NoSuchResourceError("No region with this name found"));
            }
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

        [HttpPatch("{regionName}")]
        public ActionResult<RegionDTO> PatchRegion(string regionName, RegionDTO dto)
        {
            System.Console.WriteLine($"{nameof(RegionsController)}::{nameof(PatchRegion)}({dto})");

            try
            {
                var updatedRegion = this.processor.UpdateRegion(regionName, dto);
                return new RegionDTO(updatedRegion);
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new NoSuchResourceError("No region with this name found"));
            }
            catch (DbUpdateException)
            {
                return BadRequest(new DatabaseError("Update failed; Maybe you tried to update the name to one that's already taken?"));
            }
        }

        [HttpDelete("{regionName}")]
        public ActionResult DeleteRegion(string regionName)
        {
            System.Console.WriteLine($"{nameof(RegionsController)}::{nameof(DeleteRegion)}({regionName})");

            try
            {
                this.processor.DeleteRegion(regionName);
                return Accepted();
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new NoSuchResourceError("No region with this name found"));
            }
            catch (DbUpdateException exception)
            {
                System.Console.WriteLine(exception);
                return BadRequest(new DatabaseError("Delete failed: Unknown database error"));
            }
        }
    }
}
