using RSMS.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RSMS.ViewModels
{
    public class ProductRegistrationModel : Domain.Models.ProductInfo, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validate ProductId (Assuming alphanumeric with a length between 3 and 20 characters)
            if (ProductId == null || !Regex.Match(ProductId, @"^P\d{4}$").Success)
                results.Add(new ValidationResult("Product ID should start with P, followed by 4 digits", [nameof(ProductId)]));
            else if (DatabaseService.GetProductInfoFromID(ProductId) != null)
                results.Add(new ValidationResult("Product ID is already added", [nameof(ProductId)]));
            // Validate Name (Assuming alphanumeric with a length between 3 and 50 characters)
            if (Name == null || !Regex.Match(Name, @"^[a-zA-Z0-9\s]{3,50}$").Success)
                results.Add(new ValidationResult("Name should be alphanumeric and between 3-50 characters long", [nameof(Name)]));

            // Validate Description (Assuming alphanumeric with a length between 0 and 100 characters)
            if (Description == null || !Regex.Match(Description, @"^[a-zA-Z0-9\s]{0,100}$").Success)
                results.Add(new ValidationResult("Description should be alphanumeric and up to 100 characters long", [nameof(Description)]));

            // Validate PriceBeforeTax (Assuming it should be a positive integer)
            if (PriceBeforeTax <= 0)
                results.Add(new ValidationResult("PriceBeforeTax should be a positive integer", [nameof(PriceBeforeTax)]));
            return results;
        }

    }

}
