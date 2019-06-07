using Lagerhaus.Errors;
using LagerhausServer.DTOs;
using LagerhausServer.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagerhausServer.Validation
{
    public class WeatherValidation
    {
        public ValidationError ValidateWeatherDTO(WeatherDTO dto)
        {
            if (dto.Region == null)
                return new ValidationError("Regionname cannot be null");
            if (dto.Year == null)
                return new ValidationError("Year cannot be null");
            if (dto.Month == null)
                return new ValidationError("Month cannot be null");
            if (dto.Month < 1 || dto.Month > 12)
                return new InvalidMonthError("Month cannot be null");



            if (dto.Region.Trim().Length == 0)
                return new ValidationError("Regionname cannot be empty");
            
            return null;
        }
    }
}
