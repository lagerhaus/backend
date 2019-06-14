using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lagerhaus.Errors;
using LagerhausDb;
using Lagerhaus.DTOs;
using Lagerhaus.Processors;
using Lagerhaus.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lagerhaus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchesController : ControllerBase
    {


        private BatchesValidation validation;
        private BatchesProcessor processor;

        public BatchesController(BatchesValidation validation, BatchesProcessor processor)
        {
            this.validation = validation;
            this.processor = processor;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BatchDTO>> GetAllBatches([FromQuery]String fruit, [FromQuery]int month,[FromQuery]int year, [FromQuery]String search, [FromQuery]String order, [FromQuery]bool order_desc)
        {
           
            IEnumerable<Batch> batches = this.processor.GetAllBatches();
            System.Console.WriteLine($"BatchesController::GetBatch({batches.Count()})");


            var fruitError = this.validation.ValidateString(fruit);
            if (fruitError == null)
            {
                batches = processor.filterByFruit(batches, fruit);
                System.Console.WriteLine($"BatchesController::GetBatch::FruitFilter({batches.Count()})");
            }

            var monthError = this.validation.ValidateMonth(month);
            if (monthError == null)
            {
                batches = processor.filterByMonth(batches, month);
                System.Console.WriteLine($"BatchesController::GetBatch::MonthFilter({batches.Count()})");

            }

            var yearError = this.validation.ValidateYear(year);
            if (yearError == null)
            {
                batches = processor.filterByYear(batches, year);
                System.Console.WriteLine($"BatchesController::GetBatch::YearFilter({batches.Count()})");
            }

            var searchError = this.validation.ValidateString(search);
            if (searchError == null)
            {
                batches = processor.filterBySearch(batches, search);
                System.Console.WriteLine($"BatchesController::GetBatch:SearchFilter({batches.Count()})");

            }




            if (order != null && order.Length > 0)
            {
                var validationError = this.validation.ValidateOrderBy(order);
                if (validationError != null)
                    return BadRequest(validationError);
                batches = processor.order(batches, order, order_desc);
            }
           

            return batches.Select(x=>new BatchDTO(x)).ToList();
        }

        [HttpGet("{fruit_name}/{year}/{month}")]
        public ActionResult<BatchDTO> GetBatch([FromRoute] string fruit_name, [FromRoute] int year, [FromRoute] int month)
        {
            try
            {
                var batch = this.processor.GetSingleBatch(fruit_name,year,month);
                return new BatchDTO(batch);
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new NoSuchResourceError("No batch with this name found"));
            }
        }

        [HttpPost]
        public ActionResult<BatchDTO> PostBatch([FromBody] BatchDTO dto)
        {
            System.Console.WriteLine($"BatchesController::PostBatch({dto})");

            var validationError = this.validation.ValidateBatchCreationDTO(dto);
            if (validationError != null)
                return BadRequest(validationError);

            try
            {
                var batch = this.processor.InsertBatch(dto);
                return new BatchDTO(batch);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new DuplicateKeyError("A batch with this name already exists"));
            }
        }


        [HttpPatch("{fruit_name}/{year}/{month}")]
        public ActionResult<BatchDTO> PatchBatch([FromRoute] string fruit_name, [FromRoute] int year, [FromRoute] int month, BatchDTO dto)
        {
            System.Console.WriteLine($"{nameof(BatchesController)}::{nameof(PatchBatch)}({dto})");

            try
            {
                var updatedBatch = this.processor.UpdateBatch(fruit_name,year,month, dto);
                return new BatchDTO(updatedBatch);
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new NoSuchResourceError("No batch with this name,year,month found"));
            }
            catch (DbUpdateException)
            {
                return BadRequest(new DatabaseError("Update failed; Maybe you tried to update the name,year,month to one that's already taken?"));
            }
        }

        [HttpDelete("{fruit_name}/{year}/{month}")]
        public ActionResult DeleteBatch([FromRoute] string fruit_name, [FromRoute] int year, [FromRoute] int month)
        {
            System.Console.WriteLine($"{nameof(BatchesController)}::{nameof(DeleteBatch)}({fruit_name})");

            try
            {
                this.processor.DeleteBatch(fruit_name,year,month);
                return Accepted();
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new NoSuchResourceError("No batch with this name,year,month found"));
            }
            catch (DbUpdateException exception)
            {
                System.Console.WriteLine(exception);
                return BadRequest(new DatabaseError("Delete failed: Unknown database error"));
            }
        }

    }
}
