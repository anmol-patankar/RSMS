using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RSMS.ViewModels
{
    public class UserLoginModel : IValidatableObject
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (!Regex.Match(Username, "^[a-zA-Z][a-zA-Z0-9_-]{2,19}$").Success) results.Add(new ValidationResult("Username should be between 3-20 characters long", [nameof(Username)]));
            return results;

        }
    }
}
