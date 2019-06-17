using Lagerhaus.Errors;
using Lagerhaus.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lagerhaus.Validation
{
    public class BatchesValidation
    {
        
            public ValidationError ValidateBatchCreationDTO(BatchDTO dto)
            {
           // if (dto.Month == null || dto.Year == null)
            

            if (dto.FruitName == null)
                    return new ValidationError("Name cannot be null");

                if (dto.FruitName.Trim().Length == 0)
                    return new ValidationError("Name cannot be empty");

                return null;
            }

        public ValidationError ValidateString(String s)
        {
            // if (dto.Month == null || dto.Year == null)


            if (s == null)
                return new ValidationError("Name cannot be null");

            if (s.Trim().Length == 0)
                return new ValidationError("Name cannot be empty");

            return null;
        }

        public ValidationError ValidateMonth(int i)
        {
            // if (dto.Month == null || dto.Year == null)


            if (i > 12 || i < 1)
                return new ValidationError("Month must be [1-12]");


            return null;
        }


        public ValidationError ValidateOrderBy(String by)
        {
            switch(by)
            {
                case "month":return null; ;
                case "amount":return null; ;
                case "fruit":return null; ;
                case "region":return null; ;
                case "ripeness":return null; ;
                default:return new ValidationError("Wrong order parameter; Order can only be [month|amount|fruit|region|ripeness]");

            }
        }

        internal object ValidateYear(int year)
        {
            if (year < 1)
                return new ValidationError("Year must be bigger than 0");


            return null;
        }
    }
}
