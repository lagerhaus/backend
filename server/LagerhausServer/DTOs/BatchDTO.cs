using LagerhausDb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lagerhaus.DTOs
{
    public class BatchDTO
    {
        //public int BatchId { get; set; }
        public String Fruit_Name { get; set; } // Fruit id = name ?
        public int Year { get; set; }
        public int Month { get; set; }
        public int? Amount { get; set; }
        [JsonProperty("storage_date")]
        public String StorageDate { get; set; }
        public String Region { get; set; }
        public String Ripeness { get; set; }

        public BatchDTO()
        {
        }

        public BatchDTO(Batch b)
        {
            this.Fruit_Name = b.Fruit.Name;
            this.Year = b.Year.Value;
            this.Month = b.Month.Value;
            this.Amount = b.Amount;
            this.StorageDate = b.StorageDate.HasValue? b.StorageDate.Value.Date.ToString("yyyy'/'MM'/'dd")/**ToShortDateString()**/:null;
            this.Region = b.Region!=null? b.Region.Name:"";
            this.Ripeness =b.Ripeness!=null? b.Ripeness.Name:"";
        }

        public override string ToString()
        {
            return "Fruit_Name: " + Fruit_Name + "Year: " + Year + "Month: " + Month + "Amount: " + Amount + "StorageDate: " + StorageDate + "Region: " + Region + "Ripeness: " + Ripeness;
        }


    }
}
