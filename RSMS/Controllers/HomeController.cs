using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Domain.Models;
using RSMS.Services;
using RSMS.ViewModels;
using System.Text;
namespace RSMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RsmsTestContext _context;
        private readonly IConfiguration _config;
        public HomeController(ILogger<HomeController> logger, RsmsTestContext context, IConfiguration config)
        {
            this._context = context;
            _config = config;
            _logger = logger;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserRegistrationModel user)
        {
            byte[] aesKey = Encoding.UTF8.GetBytes(_config["AesEncryption:Key"]);
            if (ModelState.IsValid)
            {
                RegistrationService.SetDBContext(_context);
                RegistrationService.RegisterUser(user, aesKey, out UserInfo userInfo, UserRoles.Customer);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                           .Select(e => e.ErrorMessage)
                                           .ToList();
                return View(user);
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
                ViewBag.Success = success;
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
