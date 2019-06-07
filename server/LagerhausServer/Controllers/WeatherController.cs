using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lagerhaus.Errors;
using LagerhausDb;
using LagerhausServer.DTOs;
using LagerhausServer.Validation;
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
        private WeatherValidation validation;

        public WeatherController(LagerhausContext db,WeatherValidation validation)
        {
            this.db = db;
            this.validation = validation;

        }

        #region Utilities
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

        private Weather GetSingleWeatherByYearMonthRegion(int year, int month, string regionName)
        {
            return
            FilterByRegion(FilterByMonth(FilterByYear(db.Weather.Include(d => d.Region), year), month), regionName)
                .SingleOrDefault();
        }
        #endregion

        #region Get
        [HttpGet]
        public ActionResult<IEnumerable<WeatherDTO>> GetAllWeathers()
        {
            return this.db.Weather.Include(d => d.Region)
                .Select(r => new WeatherDTO(r))
                .ToList();
        }
        #endregion

        #region Get :year
        [HttpGet("{year}")]
        public ActionResult<IEnumerable<WeatherDTO>> GetWeather([FromRoute] int year)
        {
            List<WeatherDTO> w = FilterByYear(db.Weather.Include(d => d.Region), year)
                .Select(r => new WeatherDTO(r)).ToList();

            if (w == null || w.Count < 1)
                return BadRequest(new NoSuchResourceError("No weather with this year found!"));

            return w;
        }
        #endregion

        #region Get :year :month
        [HttpGet("{year}/{month}")]
        public ActionResult<IEnumerable<WeatherDTO>> GetWeather([FromRoute] int year, [FromRoute] int month)
        {
            List<WeatherDTO> w = FilterByMonth(FilterByYear(db.Weather.Include(d => d.Region), year), month)
                .Select(r => new WeatherDTO(r)).ToList();

            if (w == null || w.Count < 1)
                return BadRequest(new NoSuchResourceError("No weather with this year and month found!"));

            return w;
        }
        #endregion

        #region Get :year :month :region_name
        [HttpGet("{year}/{month}/{region_name}")]
        public ActionResult<WeatherDTO> GetWeather([FromRoute] int year, [FromRoute] int month, [FromRoute] string region_name)
        {
            WeatherDTO w = new  WeatherDTO(GetSingleWeatherByYearMonthRegion(year, month, region_name));

            if (w == null /* || w.Count < 1 */)
                return BadRequest(new NoSuchResourceError("No weather with this year and month found!"));

            return w;
        }
        #endregion

        #region Post
        [HttpPost]
        public ActionResult<WeatherDTO> PostWeather([FromBody] WeatherDTO dto)
        {
          //  System.Console.WriteLine($"RegionsController::PostRegion({dto})");

            var validationError = this.validation.ValidateWeatherDTO(dto);
            if (validationError != null)
                return BadRequest(validationError);

            var w = dto.ToWeather(db);

            try
            {
                this.db.Weather.Add(w);
                this.db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new DuplicateKeyError("A weather with this name already exists"));
            }

            return new WeatherDTO(w);
        }
        #endregion

        #region Delete :year :month :region_name
        [HttpDelete("{year}/{month}/{region_name}")]
        public ActionResult DeleteWeather([FromRoute] int year, [FromRoute] int month, [FromRoute] string region_name)
        {
            //System.Console.WriteLine($"{nameof(RegionsController)}::{nameof(DeleteRegion)}({regionName})");

            try
            {
                Weather w = GetSingleWeatherByYearMonthRegion(year, month, region_name);
                this.db.Remove(w);
                this.db.SaveChanges();
                return Accepted();
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new NoSuchResourceError("No weatger with this name found"));
            }
            catch (DbUpdateException exception)
            {
                System.Console.WriteLine(exception);
                return BadRequest(new DatabaseError("Delete failed: Unknown database error"));
            }
        }
        #endregion

        #region Patch :year :month :region_name
        [HttpPatch("{year}/{month}/{region_name}")]
        public ActionResult<WeatherDTO> PatchRegion([FromRoute] int year, [FromRoute] int month, [FromRoute] string region_name, WeatherDTO dto)
        {
            //System.Console.WriteLine($"{nameof(RegionsController)}::{nameof(PatchRegion)}({dto})");

            try
            {
                Weather w = GetSingleWeatherByYearMonthRegion(year, month, region_name);
                if (dto.sunny_days != null) w.SunnyDays = dto.sunny_days;
                if (dto.rainy_days!=null)w.RainyDays = dto.rainy_days;
                this.db.Update(w);
                this.db.SaveChanges();
                return new WeatherDTO(w);
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
        #endregion

    }
}