using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using RSMS.ActionAttributes;
using RSMS.Services;
using RSMS.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RSMS.Controllers
{
    [Authorize(Roles = "Admin")]
    [NoCache]
    public class AdminController : Controller
    {
        public AdminController(RsmsTestContext context, IConfiguration config)
        {
            SecurityService.SetKeyConfig((config["AesEncryption:Key"]), config);
            DatabaseService.SetContext(context);
        }

        public IActionResult Dashboard()
        {
            List<UserInfo> allUsers = DatabaseService.GetAllUsers();
            List<Store> allStores = DatabaseService.GetAllStores();
            List<UserRoleView> allRoleMaps = new();
            foreach (var user in allUsers)
            {
                var currentUserRoles = DatabaseService.GetRolesOfUser(user.Username);
                foreach (var currentUser in currentUserRoles)
                {
                    allRoleMaps.Add(new UserRoleView { UserId = user.UserId, Username = user.Username, RoleName = currentUser.RoleName });
                }
            }

            ViewData["User"] = allUsers;
            ViewData["Store"] = allStores;
            ViewData["RoleMap"] = allRoleMaps;
            return View();
        }

        [HttpPost]
        public IActionResult DeleteUser(Guid userId)
        {
            DatabaseService.DeleteUser(userId);
            return RedirectToAction("Dashboard", "Admin");
        }
        public IActionResult EditUser(Guid userId)
        {
            return View(DatabaseService.GetUser(userId));
        }
        [HttpPost]
        public IActionResult EditUser(UserInfo userInfo)
        {

            //UserRegistrationModel userValidation = new UserRegistrationModel()
            //{
            //    Username = userInfo.Username,
            //    FirstName = userInfo.FirstName,
            //    LastName = userInfo.LastName,
            //    Email = userInfo.Email,
            //    Password = userInfo.PasswordHashed.ToString(),
            //    Phone = userInfo.Phone

            //};
            //var validationResults = userValidation.Validate(new ValidationContext(userInfo));
            //foreach (var validationResult in validationResults)
            //    ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);

            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            if (userInfo.Username == null || !Regex.Match(userInfo.Username, "^[a-zA-Z][a-zA-Z0-9_-]{2,19}$").Success) ModelState.AddModelError(nameof(userInfo.Username), "Username should be between 3-20 characters long, should start with a letter, and shouldn't have special characters");
            if (userInfo.FirstName == null || !Regex.Match(userInfo.FirstName, "^[a-zA-Z'-]+$").Success) ModelState.AddModelError(nameof(userInfo.FirstName), "Name can only have the English alphabet, hyphens, and apostrophes");
            if (userInfo.LastName == null || !Regex.Match(userInfo.LastName, "^[a-zA-Z'-]+$").Success) ModelState.AddModelError(nameof(userInfo.LastName), "Name can only have the English alphabet, hyphens, and apostrophes");
            if (userInfo.Email == null || !Regex.Match(userInfo.Email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$").Success) ModelState.AddModelError(nameof(userInfo.Email), "Invalid email adress");
            if (userInfo.Phone == null || !Regex.Match(userInfo.Phone, "^\\d{10}(?:\\d{3})?$").Success) ModelState.AddModelError(nameof(userInfo.Phone), "Phone number should be 10 digits long in case of domestic phone number, or 13 digits without '+' sign in case of international phone number");
            if (userInfo.StoreId == null || userInfo.StoreId < 0 || !DatabaseService.GetAllStores().Any(store => store.StoreId == userInfo.StoreId)) ModelState.AddModelError(nameof(userInfo.StoreId), "Store ID is not of valid store");

            if (userInfo.Dob.ToString() == null || userInfo.Dob.CompareTo(dateNow) > -1) ModelState.AddModelError(nameof(userInfo.Dob), "Date of birth should be a valid date");
            else if (userInfo.Dob.AddYears(122).CompareTo(dateNow) < 0) ModelState.AddModelError(nameof(userInfo.Dob), "User is not that old, input a valid date");
            else if (userInfo.Dob.AddYears(18).CompareTo(dateNow) > -1) ModelState.AddModelError(nameof(userInfo.Dob), "User must be atleast 18 years old ");
            if (ModelState.IsValid)
            {
                DatabaseService.EditUser(userInfo);
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(userInfo);
        }
    }
}