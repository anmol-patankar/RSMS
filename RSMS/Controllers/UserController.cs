using Domain.Models;
using Microsoft.AspNetCore.Mvc;
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
            EncryptionService.SetKey(Encoding.UTF8.GetBytes(config["AesEncryption:Key"]));
            DatabaseService.SetContext(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegistrationModel user)
        {
            ModelState.Clear();

            var validationResults = user.Validate(new ValidationContext(user));
            foreach (var validationResult in validationResults)
                ModelState.AddModelError(validationResult.MemberNames.FirstOrDefault(), validationResult.ErrorMessage);

            if (ModelState.IsValid)
            {
                var errors = DatabaseService.IsDuplicateUserRegistration(user);
                if (errors.Any())
                {
                    foreach (var error in errors)
                        ModelState.AddModelError(error.MemberNames.FirstOrDefault(), error.ErrorMessage);
                    return View(user);
                }

                var userGuid = Guid.NewGuid();
                var passwordHashed = EncryptionService.HashPassword(user.Password, out byte[] aesIV);
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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginModel login)
        {
            if (ModelState.IsValid)
            {
                bool success = DatabaseService.LoginUser(login);
                return Json(new { success });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
