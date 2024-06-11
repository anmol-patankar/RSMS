using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RSMS.ViewModels
{
    public class StoreRegistrationModel : Domain.Models.Store, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (Address == null || !Regex.Match(Address, @"^[a-zA-Z0-9\s,'-]+$").Success) results.Add(new ValidationResult("Address should be between 3-20 characters long, and shouldn't have special characters", [nameof(Address)]));
            if (Rent == null || Rent < 0) results.Add(new ValidationResult("Rent should be not less than 0", [nameof(Rent)]));
            return results;

        }
    }

}
