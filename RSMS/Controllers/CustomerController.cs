using Microsoft.AspNetCore.Mvc;

namespace RSMS.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
