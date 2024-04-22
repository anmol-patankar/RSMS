using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace RSMS.ViewModels
{
    public class UserRegistrationModel : IValidatableObject
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateOnly Dob { get; set; }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            var results = new List<ValidationResult>();
            if (!Regex.Match(Username, "^[a-zA-Z][a-zA-Z0-9_-]{2,19}$").Success) results.Add(new ValidationResult("Username should be between 3-20 characters long, should start with a letter, and shouldn't have special characters", new[] { nameof(Username) }));
            if (!Regex.Match(FirstName, "^[a-zA-Z'-]+$").Success) results.Add(new ValidationResult("Name can only have the English alphabet, hyphens, and apostrophes", new[] { nameof(FirstName) }));
            if (!Regex.Match(LastName, "^[a-zA-Z'-]+$").Success) results.Add(new ValidationResult("Name can only have the English alphabet, hyphens, and apostrophes", new[] { nameof(LastName) }));
            if (!Regex.Match(Email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$").Success) results.Add(new ValidationResult("Invalid email adress", new[] { nameof(Email) }));
            if (!Regex.Match(Password, "^.{4,}$").Success) results.Add(new ValidationResult("Password should be atleast 4 characters long", new[] { nameof(Password) }));
            if (!Regex.Match(Phone, "^\\d{10}(?:\\d{3})?$").Success) results.Add(new ValidationResult("Phone number should be 10 digits long in case of domestic phone number, or 13 digits without '+' sign in case of international phone number", new[] { nameof(Phone) }));
            if (Dob.CompareTo(dateNow) > -1) results.Add(new ValidationResult("Date of birth should be a valid date", new[] { nameof(Dob) }));
            else if (Dob.AddYears(122).CompareTo(dateNow) < 0) results.Add(new ValidationResult("You are not that old, input a valid date", new[] { nameof(Dob) }));
            else if (Dob.AddYears(18).CompareTo(dateNow) > -1) results.Add(new ValidationResult("You must be atleast 18 years old to register", new[] { nameof(Dob) }));

            return results;

        }
    }
    public enum UserRoles
    {
        Customer, Manager, Admin, Employee
    }
}
