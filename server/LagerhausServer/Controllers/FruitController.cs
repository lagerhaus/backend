using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lagerhaus.Errors;
using LagerhausDb;
using LagerhausServer.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagerhausServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitController : ControllerBase
    {
        /**
        private FruitValidation validation;
        private FruitProcessor processor;

        public FruitController(FruitValidation validation, FruitProcessor processor)
        {
            this.validation = validation;
            this.processor = processor;
        }
    **/
        LagerhausContext db;
        public FruitController(LagerhausContext lc)
        {
            db = lc;
        }

        public IEnumerable<Fruit> GetAllFruits()
        {
            return db.Fruit.Include("Ripeness");
        }

        public ValidationError ValidateFruitCreationDTO(FruitDTO dto)
        {
            if (dto.Name == null)
                return new ValidationError("Name cannot be null");

            if (dto.Name.Trim().Length == 0)
                return new ValidationError("Name cannot be empty");

            return null;
        }

        public Fruit InsertFruit(FruitDTO dto)
        {
            //Console.WriteLine("INSERT FRUIT " + dto.Name);
            var fruit = new Fruit
            {
                Name = dto.Name,
                Ripeness = new List<Ripeness>()
            };

            this.db.Fruit.Add(fruit);
            dto.Ripeness/**.Select(x => db.Ripeness.Where(y => x.Name.ToLower() == y.Name).First())**/.ToList()
                .ForEach(x => InsertRipeness(fruit,x));


            
            this.db.SaveChanges();

            return fruit;
        }

        private void InsertRipeness(Fruit f,RipenessDTO r)
        {
            //Console.WriteLine("INSERT RIPENESS " + r.Name);
            var ripe = new Ripeness
            {
                Name = r.Name,
                MinimumStorageSpan = r.minimumStorageSpan,
                Fruit = f,
                FruitId = f.FruitId
            };

            f.Ripeness.Add(ripe);

            this.db.Ripeness.Add(ripe);
            this.db.SaveChanges();            
        }

        [HttpGet]
        public ActionResult<IEnumerable<FruitDTO>> GetFruits()
        {
            return GetAllFruits()
                .Select(r => new FruitDTO(r))
                .ToList();
        }

        [HttpPost]
        public ActionResult<FruitDTO> PostFruit([FromBody] FruitDTO dto)
        {
            System.Console.WriteLine($"FruitsController::PostFruit({dto})");

            var validationError = ValidateFruitCreationDTO(dto);
            if (validationError != null)
                return BadRequest(validationError);

            try
            {
               // Console.WriteLine("BEFORE INSERT");
                var fruit = InsertFruit(dto);
                //Console.WriteLine("After insert");
                return new FruitDTO(fruit);
            }
            catch (DbUpdateException exc)
            {
                
                return BadRequest(new DuplicateKeyError("A fruit with this name already exists"));
            }
        }
    }
}