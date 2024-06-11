using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RSMS.ViewModels
{
    public class TransactionRegistrationModel : TotalTransactionModel, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            foreach (var transactionEntry in TransactionDetailList)
            {
                if (transactionEntry.Quantity <= 0)
                    results.Add(new ValidationResult("Quantity should be a positive integer.", [nameof(transactionEntry.Quantity)]));

            }
            return results;
        }

    }

}
