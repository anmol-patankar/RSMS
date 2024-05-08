using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.Services;
using RSMS.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RSMS.Controllers
{
    public class UserController : Controller
    {
        public UserController(RsmsTestContext context, IConfiguration config)
        {
            SecurityService.SetKeyConfig((config["AesEncryption:Key"]), config);
            DatabaseService.SetContext(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("SelectUserType");
            return View();
        }

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
                    return RedirectToAction("SelectUserType", "User");
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
                    RegistrationDate = DateTime.Now,
                    StoreId = 0
                };
                var roleMap = new RoleMap()
                {
                    UserId = userGuid,
                    RoleName = DatabaseService.UserRoles.Customer.ToString()
                };
                DatabaseService.AddUserAndRole(userInfo, roleMap);
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        [Authorize]
        public IActionResult SelectUserType()
        {
            var roles = User.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value).ToList();
            if (roles.Count > 1)
            {
                ViewBag.Roles = roles;
                return View();
            }
            return RedirectToAction("Dashboard", roles.First());
        }

        [HttpPost]
        public IActionResult SelectUserType(string selectedRole)
        {
            if (!string.IsNullOrEmpty(selectedRole)) return RedirectToAction("Dashboard", selectedRole);
            return RedirectToAction("Login", "User");
        }

        internal IActionResult AuthenticatedRedirect()
        {
            var userRoles = User.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value).ToList();
            if (userRoles.Count > 1)
            {
                ViewBag.Roles = userRoles;
                return View("SelectUserType");
            }
            return RedirectToAction("Dashboard", userRoles.First());
        }
    }
}