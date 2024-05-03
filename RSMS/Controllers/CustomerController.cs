using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.ActionAttributes;

namespace RSMS.Controllers
{
    [Authorize(Roles = "Admin,Customer")]
    [NoCache]
    public class CustomerController : Controller
    {
        public IActionResult Dashboard()
        {
            ViewBag.User = HttpContext.User.Identity?.Name;
            return View();
        }
    }
}