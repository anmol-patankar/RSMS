using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Domain.Models;
using RSMS.Services;
using RSMS.ViewModels;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace RSMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly RsmsTestContext _context;
        private readonly IConfiguration _config;
        public HomeController(RsmsTestContext context, IConfiguration config)
        {
            this._context = context;
            _config = config;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserRegistrationModel user)
        {

            byte[] aesKey = Encoding.UTF8.GetBytes(_config["AesEncryption:Key"]);
            if (!ModelState.IsValid)
                return View(user);
            else
            {
                RegistrationService.SetDBContext(_context);
                RegistrationService.RegisterUser(user, aesKey, out UserInfo userInfo, UserRoles.Customer);
                foreach (var error in RegistrationService.Errors)
                    ModelState.AddModelError(error.MemberNames.FirstOrDefault(), error.ErrorMessage);
                if (RegistrationService.Errors.Count != 0)
                    return View(user);
                else
                    return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel login)
        {
            byte[] aesKey = Encoding.UTF8.GetBytes(_config["AesEncryption:Key"]);
            if (ModelState.IsValid)
            {
                LoginService.SetDBContext(_context);
                bool success = LoginService.LoginUser(login, aesKey);
                var x = new { success };
                return Json(new { success });
            }

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
