using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace RSMS.Controllers
{
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login", "User");
        }
    }
}