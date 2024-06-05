using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.Services;
using RSMS.ViewModels;
using System.Text.RegularExpressions;

namespace RSMS.Controllers
{
    public class StoreController : Controller
    {
        public StoreController(RsmsTestContext context, IConfiguration config)
        {
            SecurityService.SetKeyConfig((config[Constants.AesKeyString]), config);
            DatabaseService.SetContext(context);
        }

        [HttpPost]
        public IActionResult AddProductStock(AddNewProductToStoreModel model)
        {
            if (model.Quantity < 0) model.Quantity = 0;
            if (model.DiscountPercent < 0) model.DiscountPercent = 0;

            if (ModelState.IsValid)
            {
                ProductStock stock = new()
                {
                    StoreId = model.StoreId,
                    DiscountPercent = model.DiscountPercent,
                    Quantity = model.Quantity,
                    ProductId = DatabaseService.GetProductInfoFromName(model.Name).ProductId
                };
                DatabaseService.AddProductStockToStore(stock);
            }
            return RedirectToAction("StoreProducts", "Product", new { storeId = model.StoreId });
        }

        [HttpGet]
        public IActionResult AddProductStock(int storeId)
        {
            ViewBag.NotAddedNamesList = DatabaseService.GetNotAddedProducts(storeId);
            var tempModel = new AddNewProductToStoreModel() { StoreId = storeId };
            return PartialView("_AddProductStockPartial", tempModel);
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