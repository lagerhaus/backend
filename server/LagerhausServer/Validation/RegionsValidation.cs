using Lagerhaus.DTOs;
using Lagerhaus.Errors;

namespace Lagerhaus.Validation
{
    public class RegionsValidation
    {
        public ValidationError ValidateRegionCreationDTO(RegionDTO dto)
        {
            if (dto.Name == null)
                return new ValidationError("Name cannot be null");

            if (dto.Name.Trim().Length == 0)
                return new ValidationError("Name cannot be empty");

            return null;
        }
    }
}
