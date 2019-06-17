using LagerhausDb;
using Lagerhaus.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lagerhaus.Processors
{
    public class BatchesProcessor
    {
        private LagerhausContext db;
        public BatchesProcessor(LagerhausContext db)
        {
            this.db = db;
        }

        public LagerhausContext Db { get => db; set => db = value; }

        public IEnumerable<Batch> GetAllBatches() => this.db.Batch.Include("Fruit").Include("Region").Include("Ripeness");


        public Batch GetSingleBatch(string fruit, int year, int month) => filterByMonth(filterByYear(filterByFruit(GetAllBatches(), fruit), year), month).First();
            
                
            

        public IEnumerable<Batch> filterByFruit(IEnumerable<Batch> batches, String fruitname) => batches.Where(x => x.Fruit.Name.Equals(fruitname)).ToList();
        public IEnumerable<Batch> filterByMonth(IEnumerable<Batch> batches, int month) => batches.Where(x => x.Month==month).ToList();
        public IEnumerable<Batch> filterByYear(IEnumerable<Batch> batches, int year) => batches.Where(x => x.Year==year).ToList();

        public IEnumerable<Batch> filterBySearch(IEnumerable<Batch> batches, String search) => batches.Where(x => new BatchDTO(x).ToString().ToLower().Contains(search.ToLower())).ToList();

        public object keyFromBatch(Batch b,String by)
        {

            switch(by)
            {
                case "month":return b.Month.Value;;
                case "amount":return b.Amount.Value;;
                case "fruit":return b.Fruit.Name;;
                case "region":return b.Region.Name;;
                case "ripeness":return b.Ripeness.Name;;         
            }
            throw new InvalidOperationException();
        }
        public IEnumerable<Batch> order(IEnumerable<Batch> batches, String by, bool desc) => desc ? batches.OrderByDescending(x=>keyFromBatch(x,by)) : batches.OrderBy(x=>keyFromBatch(x,by));

        public Batch InsertBatch(BatchDTO b)
        {
            Batch batch = BatchFromDTO(b);

            this.db.Batch.Add(batch);
            this.db.SaveChanges();

            return batch;
        }

        public Batch BatchFromDTO(BatchDTO b)
        {
            var batch = new Batch
            {
                /**
                Name = dto.Name,
                Area = dto.Area,
                Level = dto.Level
                **/
                Fruit = db.Fruit.Single(x => x.Name == b.FruitName),
                Year = b.Year,
                Month = b.Month,
                Amount = b.Amount,
                StorageDate = DateTime.ParseExact(b.StorageDate, "yyyy/MM/dd", null),
                Region = db.Region.Single(x => x.Name == b.Region),
                Ripeness = db.Ripeness.Single(x => x.Name == b.Ripeness)
            };
            return batch;
        }

        
        public Batch UpdateBatch(string fruit, int year, int month, BatchDTO dto)
        {
            var batch = GetSingleBatch(fruit,year,month);
            // BatchDTO dbDto = new BatchDTO(batch);
            

            if (dto.FruitName != null) batch.Fruit = db.Fruit.Single(x => x.Name == dto.FruitName);
            if (dto.Year != 0) batch.Year = dto.Year;
            if (dto.Month != 0) batch.Month = dto.Month;
            if (dto.Amount != 0) batch.Amount = dto.Amount;
            if (dto.StorageDate != null) batch.StorageDate = DateTime.ParseExact(dto.StorageDate, "yyyy/MM/dd", null);
            if (dto.Region != null) batch.Region = db.Region.Single(x => x.Name.Equals(dto.Region));
            if (dto.Ripeness != null) batch.Ripeness = db.Ripeness.Single(x => x.Name.Equals(dto.Ripeness));


            /**
            if (dto.Fruit_Name != null)
                batch.Name = dto.Name;
            if (dto.Area != null)
                batch.Area = dto.Area;
            if (dto.Level != null)
                batch.Level = dto.Level;
    **/
            /**
                    if (dto.Fruit_Name != null) dbDto.Fruit_Name = dto.Fruit_Name;
                    if (dto.Year != null) dbDto.Year = dto.Year;
                    if (dto.Month != null) dbDto.Month = dto.Month;
                    if (dto.Amount != null) dbDto.Amount = dto.Amount;
                    if (dto.Storage_Date != null) dbDto.Storage_Date = dto.Storage_Date;
                    if (dto.Region != null) dbDto.Region = dto.Region;
                    if (dto.Ripeness != null) dbDto.Ripeness = dto.Ripeness;
            **/

           // batch = BatchFromDTO(dbDto);

            this.db.Update(batch);
            this.db.SaveChanges();

            return batch;
        }
    

        public void DeleteBatch(string fruit, int year, int month)
        {
            var batch = GetSingleBatch(fruit,year,month);
            this.db.Remove(batch);
            this.db.SaveChanges();
        }
    }
}
