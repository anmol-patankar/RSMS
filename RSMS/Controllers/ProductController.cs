using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using RSMS.Services;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using RSMS.ActionAttributes;
namespace RSMS.Controllers
{
    [Authorize]
    [NoCache]
    public class ProductController : Controller
    {
        public ProductController(RsmsTestContext context, IConfiguration config)
        {
            SecurityService.SetKeyConfig((config[Constants.AesKeyString]), config);
            DatabaseService.SetContext(context);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StoreProducts(int storeId)
        {
            var productsOfStore = DatabaseService.GetTotalProductInfo(storeId);
            ViewBag.StoreId = storeId;
            ViewBag.CurrentUser = DatabaseService.GetUser(User.Identity.Name);
            return View(productsOfStore);
        }
        [HttpPost]
        public IActionResult UpdateStoreQtyAndDiscount(int storeId, string productId, int quantity, int discountPercent)
        {
            if (quantity < 0) quantity = 0;
            if (discountPercent < 0) discountPercent = 0;
            else if (discountPercent > 100) discountPercent = 100;
            DatabaseService.UpdateProductStock(storeId, productId, quantity);
            DatabaseService.UpdateProductDiscount(storeId, productId, discountPercent);

            var result = new
            {
                Quantity = quantity,
                DiscountPercent = discountPercent
            };
            var json = JsonSerializer.Serialize(result);
            return Ok(json);
        }
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult EditProductInfo(string productId)
        {
            return View(DatabaseService.GetProductInfoFromID(productId));
        }
        [HttpPost]
        public IActionResult EditProductInfo(ProductInfo productInfo)
        {
            if (productInfo.Name == null || productInfo.Name == "" || !Regex.Match(productInfo.Name, "^[a-zA-Z0-9 ]{3,25}$").Success) ModelState.AddModelError("Name", "Name should have only alphabets, numbers, and spaces");
            if (productInfo.Description == null || productInfo.Description == "" || !Regex.Match(productInfo.Description, @"^[a-zA-Z0-9\s\-,.!?()'"":;\/]+$").Success) ModelState.AddModelError("Description", "Description must only have letters, numbers, and puctuation");
            if (productInfo.PriceBeforeTax < 0) ModelState.AddModelError("PriceBeforeTax", "Price before taxation must atleast be greater than zero");
            if (ModelState.IsValid)
            {
                DatabaseService.EditProductInfo(productInfo);
                return RedirectToAction("Dashboard", "User");
            }
            return View(productInfo);
        }
    }
}
