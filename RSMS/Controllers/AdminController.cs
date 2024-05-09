using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.ActionAttributes;
using RSMS.Services;
using RSMS.ViewModels;
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
                foreach (var currentRole in currentUserRoles)
                {
                    allRoleMaps.Add(new UserRoleView { UserId = user.UserId, Username = user.Username, RoleName = currentRole });
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
            var userToEdit = new UserDetailsEditorModel
            {
                UserInfo = DatabaseService.GetUser(userId),
                RolesUserHas = DatabaseService.GetRolesOfUser(userId)
            };
            return View(userToEdit);
        }
        [HttpPost]
        public IActionResult EditUser(UserDetailsEditorModel userToEdit)
        {
            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            if (userToEdit.UserInfo.Username == null || !Regex.Match(userToEdit.UserInfo.Username, "^[a-zA-Z][a-zA-Z0-9_-]{2,19}$").Success) ModelState.AddModelError(("UserInfo.Username"), "Username should be between 3-20 characters long, should start with a letter, and shouldn't have special characters");
            if (userToEdit.UserInfo.FirstName == null || !Regex.Match(userToEdit.UserInfo.FirstName, "^[a-zA-Z'-]+$").Success) ModelState.AddModelError(("UserInfo.FirstName"), "Name can only have the English alphabet, hyphens, and apostrophes");
            if (userToEdit.UserInfo.LastName == null || !Regex.Match(userToEdit.UserInfo.LastName, "^[a-zA-Z'-]+$").Success) ModelState.AddModelError(("UserInfo.LastName"), "Name can only have the English alphabet, hyphens, and apostrophes");
            if (userToEdit.UserInfo.Email == null || !Regex.Match(userToEdit.UserInfo.Email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$").Success) ModelState.AddModelError(("UserInfo.Email"), "Invalid email adress");
            if (userToEdit.UserInfo.Phone == null || !Regex.Match(userToEdit.UserInfo.Phone, "^\\d{10}(?:\\d{3})?$").Success) ModelState.AddModelError("UserInfo.Phone", "Phone number should be 10 digits long in case of domestic phone number, or 13 digits without '+' sign in case of international phone number");
            if (userToEdit.UserInfo.StoreId == null || userToEdit.UserInfo.StoreId < 0 || !DatabaseService.GetAllStores().Any(store => store.StoreId == userToEdit.UserInfo.StoreId)) ModelState.AddModelError(("UserInfo.StoreId"), "Store ID is not of valid store");

            if (userToEdit.UserInfo.Dob.ToString() == null || userToEdit.UserInfo.Dob.CompareTo(dateNow) > -1) ModelState.AddModelError(("UserInfo.Dob"), "Date of birth should be a valid date");
            else if (userToEdit.UserInfo.Dob.AddYears(122).CompareTo(dateNow) < 0) ModelState.AddModelError(("UserInfo.Dob"), "User is not that old, input a valid date");
            else if (userToEdit.UserInfo.Dob.AddYears(18).CompareTo(dateNow) > -1) ModelState.AddModelError(("UserInfo.Dob"), "User must be atleast 18 years old ");
            if (userToEdit.RolesUserHas == null || userToEdit.RolesUserHas.Count == 0) ModelState.AddModelError(nameof(userToEdit.RolesUserHas), "Atleast one role must be selected");
            if (userToEdit.RolesUserHas.Contains("Employee") && userToEdit.RolesUserHas.Contains("Manager")) ModelState.AddModelError(nameof(userToEdit.RolesUserHas), "User cannot be manager and employee at the same time");
            if (ModelState.IsValid)
            {
                DatabaseService.EditUser(userToEdit.UserInfo);
                List<string> rolesToRemove = Enum.GetNames(typeof(DatabaseService.UserRoles)).ToList();
                foreach (var selectedRole in userToEdit.RolesUserHas)
                {
                    rolesToRemove.Remove(selectedRole);
                }
                DatabaseService.EditRoles(userToEdit.RolesUserHas, rolesToRemove, userToEdit.UserInfo.UserId);
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(userToEdit);
        }

        //[HttpPost]
        //public IActionResult EditUserInfo(UserInfo userInfo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        DatabaseService.EditUser(userInfo);
        //        return RedirectToAction("Dashboard", "Admin");
        //    }
        //    return View("EditUser");
        //}
        ////TODO ERROR HANDLING BELOW ISNT WORKING DO IT ITITITITITITITITIT
        //[HttpPost]
        //public IActionResult EditUserRoles(List<string> rolesToAdd, string username)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var userGuid = DatabaseService.GetUser(username).UserId;
        //        List<string> rolesToRemove = Enum.GetNames(typeof(DatabaseService.UserRoles)).ToList();
        //        foreach (var selectedRole in rolesToAdd)
        //        {
        //            rolesToRemove.Remove(selectedRole);
        //        }
        //        DatabaseService.EditRoles(rolesToAdd, rolesToRemove, userGuid);
        //        return RedirectToAction("Dashboard", "Admin");

        //    }
        //    return PartialView("_EditUserRolesPartial", rolesToAdd);
        //}
    }
}