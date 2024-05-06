using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.Services;
using RSMS.ViewModels;

namespace RSMS.Controllers
{
    public class ManagerController : Controller
    {
        public ManagerController(RsmsTestContext context, IConfiguration config)
        {
            SecurityService.SetKeyConfig((config["AesEncryption:Key"]), config);
            DatabaseService.SetContext(context);
        }
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Dashboard()
        {
            var manager = DatabaseService.GetUser(User.Identity.Name);
            ViewBag.Employees = DatabaseService.GetAllUsers()
                .Where(u => u.StoreId == manager.StoreId
                && DatabaseService.GetRolesOfUser(u.Username).Any(role => role.RoleName == UserRoles.Employee.ToString())).ToList();
            ViewBag.Products = null;
            ViewBag.Transactions = null;
            return View();


        }
    }
}
