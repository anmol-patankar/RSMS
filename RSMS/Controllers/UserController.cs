using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.Services;
using RSMS.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace RSMS.Controllers
{

    public class UserController : Controller
    {
        public UserController(RsmsTestContext context, IConfiguration config)
        {
            SecurityService.SetKeyConfig((config["AesEncryption:Key"]), config);
            DatabaseService.SetContext(context);
        }
        [Authorize]
        public IActionResult Dashboard()
        {
            var currentAccessingUser = DatabaseService.GetUser(User.Identity.Name);
            ViewBag.CurrentUser = currentAccessingUser;
            List<UserInfo> allUsers = DatabaseService.GetAllUsers();
            ViewBag.AllUsers = allUsers;
            List<Store> allStores = DatabaseService.GetAllStores();
            ViewBag.AllStores = allStores;
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Dashboard");
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserLoginModel login)
        {
            if (ModelState.IsValid)
            {
                bool success = DatabaseService.LoginCredentialValidity(login.Username, login.Password);
                if (success)
                {
                    string token = SecurityService.GenerateEncryptedToken(login);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    };

                    HttpContext.Response.Cookies.Append("JWTToken", token, cookieOptions);
                    return RedirectToAction("Dashboard", "User");
                }
            }
            ViewBag.Message = "Invalid username or password";
            return View(login);
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("JWTToken");
            return RedirectToAction("Login", "User");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated == true)
            {
                return RedirectToAction("SelectUserType");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegistrationModel user)
        {
            ModelState.Clear();

            var validationResults = user.Validate(new ValidationContext(user));
            foreach (var validationResult in validationResults)
                ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);

            if (ModelState.IsValid)
            {
                var errors = DatabaseService.IsDuplicateUserRegistration(user);
                if (errors.Count > 0)
                {
                    foreach (var error in errors)
                        ModelState.AddModelError(error.MemberNames.First(), error.ErrorMessage);
                    return View(user);
                }

                var userGuid = Guid.NewGuid();
                var passwordHashed = SecurityService.GetHashedAndSaltedString(user.Password ?? "", out byte[] aesIV);
                var userInfo = new UserInfo()
                {
                    UserId = userGuid,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Dob = user.Dob,
                    Email = user.Email,
                    Salt = aesIV,
                    PasswordHashed = passwordHashed,
                    Phone = user.Phone,
                    RoleId = (int)(DatabaseService.UserRole)Enum.Parse(typeof(DatabaseService.UserRole), "Customer"),
                    RegistrationDate = DateTime.Now,
                    StoreId = 0
                };

                DatabaseService.AddUser(userInfo);
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        //[Authorize]
        //public IActionResult SelectUserType()
        //{
        //    var roles = User.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value).ToList();
        //    if (roles.Count > 1)
        //    {
        //        ViewBag.Roles = roles;
        //        return View();
        //    }
        //    return RedirectToAction("Dashboard", roles.First());
        //}

        //[HttpPost]
        //public IActionResult SelectUserType(string selectedRole)
        //{
        //    if (!string.IsNullOrEmpty(selectedRole)) return RedirectToAction("Dashboard", selectedRole);
        //    return RedirectToAction("Login", "User");
        //}
        //internal IActionResult AuthenticatedRedirect()
        //{
        //    var userRoles = User.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value).ToList();
        //    if (userRoles.Count > 1)
        //    {
        //        ViewBag.Roles = userRoles;
        //        return View("SelectUserType");
        //    }
        //    return RedirectToAction("Dashboard", userRoles.First());
        //}

        [HttpPost]
        public IActionResult DeleteUser(Guid userId)
        {
            DatabaseService.DeleteUser(userId);
            return RedirectToAction("Dashboard", "User");
        }
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult EditUser(Guid userId)
        {
            return View(DatabaseService.GetUser(userId));
        }

        [HttpPost]
        public IActionResult EditUser(UserInfo userToEdit)
        {
            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            if (userToEdit.Username == null || !Regex.Match(userToEdit.Username, "^[a-zA-Z][a-zA-Z0-9_-]{2,19}$").Success) ModelState.AddModelError(("Username"), "Username should be between 3-20 characters long, should start with a letter, and shouldn't have special characters");
            if (userToEdit.FirstName == null || !Regex.Match(userToEdit.FirstName, "^[a-zA-Z'-]+$").Success) ModelState.AddModelError(("FirstName"), "Name can only have the English alphabet, hyphens, and apostrophes");
            if (userToEdit.LastName == null || !Regex.Match(userToEdit.LastName, "^[a-zA-Z'-]+$").Success) ModelState.AddModelError(("LastName"), "Name can only have the English alphabet, hyphens, and apostrophes");
            if (userToEdit.Email == null || !Regex.Match(userToEdit.Email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$").Success) ModelState.AddModelError(("Email"), "Invalid email adress");
            if (userToEdit.Phone == null || !Regex.Match(userToEdit.Phone, "^\\d{10}(?:\\d{3})?$").Success) ModelState.AddModelError("Phone", "Phone number should be 10 digits long in case of domestic phone number, or 13 digits without '+' sign in case of international phone number");
            if (userToEdit.StoreId == null || userToEdit.StoreId < 0 || !DatabaseService.GetAllStores().Any(store => store.StoreId == userToEdit.StoreId)) ModelState.AddModelError(("StoreId"), "Store ID is not of valid store");

            if (userToEdit.Dob.ToString() == null || userToEdit.Dob.CompareTo(dateNow) > -1) ModelState.AddModelError(("Dob"), "Date of birth should be a valid date");
            else if (userToEdit.Dob.AddYears(122).CompareTo(dateNow) < 0) ModelState.AddModelError(("Dob"), "User is not that old, input a valid date");
            else if (userToEdit.Dob.AddYears(18).CompareTo(dateNow) > -1) ModelState.AddModelError(("Dob"), "User must be atleast 18 years old ");
            string roleErrors = "";
            if (userToEdit.Username == User.Identity.Name)
            {
                roleErrors += "You cannot edit your own privileges. ";
            }
            else
            {
                var roleIdOfEditingUser = (int)(DatabaseService.UserRole)Enum.Parse(typeof(DatabaseService.UserRole), HttpContext.User.FindFirst(ClaimTypes.Role).Value);
                if (userToEdit.RoleId > roleIdOfEditingUser)
                    roleErrors += "You cannot add privileges higher than your own. ";
            }
            if (roleErrors != "") ModelState.AddModelError(("RoleId"), roleErrors);

            if (ModelState.IsValid)
            {
                DatabaseService.EditUser(userToEdit);
                return RedirectToAction("Dashboard", "User");
            }
            return View(userToEdit);
        }
    }
}