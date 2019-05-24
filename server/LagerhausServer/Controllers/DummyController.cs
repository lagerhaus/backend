using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LagerhausDb;
using Microsoft.AspNetCore.Mvc;

namespace LagerhausServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyController : ControllerBase
    {
        private LagerhausContext db;

        public DummyController(LagerhausContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return this.db.Fruit.Select(f => f.Name).ToList();
        }
    }
}
