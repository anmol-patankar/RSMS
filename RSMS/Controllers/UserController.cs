﻿using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.ActionAttributes;
using RSMS.Services;
using RSMS.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RSMS.Controllers
{
    public class UserController : Controller
    {
        public UserController(RsmsTestContext context, IConfiguration config)
        {
            SecurityService.SetKeyConfig(config[Constants.AesKeyString], config);
            DatabaseService.SetContext(context);
        }

        [Authorize]
        [NoCache]
        public IActionResult Dashboard()
        {
            var currentAccessingUser = DatabaseService.GetUser(User.Identity.Name);
            ViewBag.CurrentUser = currentAccessingUser;
            IEnumerable<UserInfo> allUsers = DatabaseService.GetAllUsers();
            ViewBag.AllUsers = allUsers;
            IEnumerable<Store> allStores = DatabaseService.GetAllStores();
            ViewBag.AllStores = allStores;
            IEnumerable<ProductInfo> productInfos = DatabaseService.GetAllProductInfos();
            ViewBag.ProductInfos = productInfos;
            var allTransactions = currentAccessingUser.RoleId == 2
                ? DatabaseService.GetAllTransactions(currentAccessingUser.UserId)
                : DatabaseService.GetAllTransactions();
            ViewBag.TransactionsByStore = allTransactions
                .GroupBy(t => t.StoreId)
                .OrderBy(g => g.Key);


            return View();
        }

        [Authorize]
        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
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
            bool success = DatabaseService.LoginCredentialValidity(login.Username, login.Password);
            if (!success)
            {
                ModelState.AddModelError("Username", "Invalid or incorrect username or password");
            }
            if (ModelState.IsValid)
            {

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
            if (User.Identity != null && User.Identity.IsAuthenticated == true
                && !(User.IsInRole("Admin") || User.IsInRole("Manager")))
            {
                return RedirectToAction("Dashboard");
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
                    RoleId = DatabaseService.RoleNameToRoleId("Customer"),
                    RegistrationDate = DateTime.Now,
                    StoreId = 0
                };

                DatabaseService.AddUser(userInfo);
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteUser(Guid userId)
        {
            DatabaseService.DeleteUser(userId);
            return RedirectToAction("Dashboard", "User");
        }

        [Authorize]
        public IActionResult EditUser(Guid userId)
        {
            bool getNullStore = User.IsInRole("Admin");
            Dictionary<int, string> allStoreNames = DatabaseService.GetAllStores(getNullStore).ToDictionary(s => s.StoreId, s => s.Address);
            ViewBag.StoreNames = allStoreNames;
            return View(DatabaseService.GetUser(userId));
        }
        [Authorize]
        [HttpPost]
        public IActionResult EditUser(UserInfo userToEdit)
        {
            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            if (userToEdit.Username == null || !Regex.Match(userToEdit.Username, "^[a-zA-Z][a-zA-Z0-9_-]{2,19}$").Success) ModelState.AddModelError(("Username"), "Username should be between 3-20 characters long, should start with a letter, and shouldn't have special characters");
            if (userToEdit.FirstName == null || !Regex.Match(userToEdit.FirstName, "^[a-zA-Z'-]+$").Success) ModelState.AddModelError(("FirstName"), "Name can only have the English alphabet, hyphens, and apostrophes");
            if (userToEdit.LastName == null || !Regex.Match(userToEdit.LastName, "^[a-zA-Z'-]+$").Success) ModelState.AddModelError(("LastName"), "Name can only have the English alphabet, hyphens, and apostrophes");
            if (userToEdit.Email == null || !Regex.Match(userToEdit.Email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$").Success) ModelState.AddModelError(("Email"), "Invalid email adress");
            if (userToEdit.Phone == null || !Regex.Match(userToEdit.Phone, "^\\d{10}(?:\\d{3})?$").Success) ModelState.AddModelError("Phone", "Phone number should be 10 digits long in case of domestic phone number, or 13 digits without '+' sign in case of international phone number");
            if (userToEdit.StoreId == null || userToEdit.StoreId < 0 || !DatabaseService.GetAllStores(getNullStore: true).Any(store => store.StoreId == userToEdit.StoreId)) ModelState.AddModelError(("StoreId"), "Store ID is not of valid store");

            if (userToEdit.Dob.ToString() == null || userToEdit.Dob.CompareTo(dateNow) == 0) ModelState.AddModelError(("Dob"), "Date of birth should be a valid date");
            else if (userToEdit.Dob.AddYears(122).CompareTo(dateNow) < 0) ModelState.AddModelError(("Dob"), "User is not that old, input a valid date");
            else if (userToEdit.Dob.AddYears(18).CompareTo(dateNow) > -1) ModelState.AddModelError(("Dob"), "User must be atleast 18 years old ");
            var roleIdOfEditingUser = DatabaseService.RoleNameToRoleId(User.FindFirstValue(ClaimTypes.Role));
            if (userToEdit.RoleId > roleIdOfEditingUser)
            {
                ModelState.AddModelError(("RoleId"), "You cannot add privileges higher than your own. ");
            }
            if (ModelState.IsValid)
            {
                DatabaseService.EditUser(userToEdit);
                return RedirectToAction("Dashboard", "User");
            }
            return View(userToEdit);
        }
    }
}