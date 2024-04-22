using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RSMS.Services;
using RSMS.ViewModels;
using System.Text;

namespace RSMS.Controllers
{
    public class UserController : Controller
    {
        private readonly RsmsTestContext _context;
        private readonly IConfiguration _config;
        private readonly byte[] _aesKey;
        public UserController(RsmsTestContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _aesKey = Encoding.UTF8.GetBytes(_config["AesEncryption:Key"]);
            UserService.SetContextAndKey(_context, _aesKey);
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


            if (!ModelState.IsValid)
                return View(user);
            else
            {
                UserService.RegisterUser(user, UserRoles.Customer);
                foreach (var error in UserService.Errors)
                    ModelState.AddModelError(error.MemberNames.FirstOrDefault(), error.ErrorMessage);
                if (UserService.Errors.Count != 0)
                    return View(user);
                else
                    return RedirectToAction("Index", "Home");
            }
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                bool success = UserService.LoginUser(login);
                var x = new { success };
                return Json(new { success });
            }

            return RedirectToAction("Index", "Home");
        }

    }
}
