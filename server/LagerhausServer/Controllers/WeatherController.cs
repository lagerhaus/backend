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
    public class WeatherController : ControllerBase
    {
        private LagerhausContext db;

        public WeatherController(LagerhausContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WeatherDTO>> GetAllWeathers()
        {
            return this.db.Weather.Include(d => d.Region)
                .Select(r => new WeatherDTO(r))
                .ToList();
        }

        private List<Weather> FilterByYear(IEnumerable<Weather> weathers, int year)
        {
            return weathers.Where(r => r.Year == year).ToList();
        }

        private List<Weather> FilterByMonth(IEnumerable<Weather> weathers, int month)
        {
            return weathers.Where(r => r.Month == month).ToList();
        }

        private List<Weather> FilterByRegion(IEnumerable<Weather> weathers, string regionName)
        {
            regionName = regionName.ToLower();
            return weathers.Where(r => r.Region.Name.ToLower() == regionName).ToList();
        }


        [HttpGet("{year}")]
        public ActionResult<IEnumerable<WeatherDTO>> GetWeather([FromRoute] int year)
        {
            List<WeatherDTO> w = FilterByYear(db.Weather.Include(d => d.Region), year)
                .Select(r => new WeatherDTO(r)).ToList();

            if (w == null || w.Count < 1)
                return BadRequest(new NoSuchResourceError("No weather with this year found!"));

            return w;
        }

        [HttpGet("{year}/{month}")]
        public ActionResult<IEnumerable<WeatherDTO>> GetWeather([FromRoute] int year, [FromRoute] int month)
        {
            List<WeatherDTO> w = FilterByMonth(FilterByYear(db.Weather.Include(d => d.Region), year), month)
                .Select(r => new WeatherDTO(r)).ToList();

            if (w == null || w.Count < 1)
                return BadRequest(new NoSuchResourceError("No weather with this year and month found!"));

            return w;
        }
        
        [HttpGet("{year}/{month}/{region_name}")]
        public ActionResult<WeatherDTO> GetWeather([FromRoute] int year, [FromRoute] int month, [FromRoute] string region_name)
        {
            WeatherDTO w = FilterByRegion(FilterByMonth(FilterByYear(db.Weather.Include(d => d.Region), year), month), region_name)
                .Select(r => new WeatherDTO(r)).SingleOrDefault();

            if (w == null /* || w.Count < 1 */)
                return BadRequest(new NoSuchResourceError("No weather with this year and month found!"));

            return w;
        }
    }
}