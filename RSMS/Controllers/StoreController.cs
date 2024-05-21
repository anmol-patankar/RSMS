using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.Services;

namespace RSMS.Controllers
{

    public class StoreController : Controller
    {
        public StoreController(RsmsTestContext context, IConfiguration config)
        {
            SecurityService.SetKeyConfig((config["AesEncryption:Key"]), config);
            DatabaseService.SetContext(context);
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult EditStore(int storeId)
        {
            return View(DatabaseService.GetStore(storeId));
        }

        [HttpPost]
        public IActionResult DeleteStore(int storeId)
        {
            DatabaseService.DeleteStore(storeId);
            return RedirectToAction("Dashboard", "User");
        }

        [HttpPost]
        public IActionResult EditStore(Store storeToEdit)
        {
            if (storeToEdit.Address == null || storeToEdit.Address.Trim() == "")
                ModelState.AddModelError("Address", "Address cannot be empty");
            if (storeToEdit.Rent < 0)
                ModelState.AddModelError("Rent", "Rent cannot be lesser than 0");

            if (ModelState.IsValid)
            {
                DatabaseService.EditStore(storeToEdit);
                return RedirectToAction("Dashboard", "User");
            }
            return View(storeToEdit);
        }
    }
}