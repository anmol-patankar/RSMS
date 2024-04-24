using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.ActionAttribute;
using RSMS.Services;
using RSMS.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RSMS.Controllers
{

    public class UserController : Controller
    {
        //private readonly RsmsTestContext _context;
        //private readonly IConfiguration _config;

        public UserController(RsmsTestContext context, IConfiguration config)
        {
            //_context = context;
            //_config = config;
            SecurityService.SetKeyConfig(Encoding.UTF8.GetBytes(config["AesEncryption:Key"]), config);
            DatabaseService.SetContext(context);
        }

        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated == true) return RedirectToAction("LoginSuccess", "User");
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
                var passwordHashed = SecurityService.HashPassword(user.Password ?? "", out byte[] aesIV);
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
                    RoleName = UserRoles.Customer.ToString()
                };
                DatabaseService.AddUserAndRole(userInfo, roleMap);
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated == true) return RedirectToAction("LoginSuccess", "User");
            return View();
        }

        public IActionResult Logout()
        {

            //HttpContext.Response.Cookies.Delete("JWTToken");
            Response.Cookies.Delete("JWTToken");
            return RedirectToAction("Login", "User");
        }
        [NoCache]
        [Authorize(Roles = "Customer")]
        public IActionResult LoginSuccess()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserLoginModel login)
        {
            if (ModelState.IsValid)
            {
                bool success = DatabaseService.LoginUser(login);
                if (success)
                {
                    string token = SecurityService.GenerateToken(login);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    };
                    HttpContext.Response.Cookies.Append("JWTToken", token, cookieOptions);
                    //return View("LoginSuccess", login);
                    return RedirectToAction("LoginSuccess", "User");
                }
                //return Json(new { success });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
