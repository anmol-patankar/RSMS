using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using RSMS.Services;

namespace RSMS.Controllers
{
    public class TransactionController : Controller
    {
        public TransactionController(RsmsTestContext context, IConfiguration config)
        {
            SecurityService.SetKeyConfig(config[Constants.AesKeyString], config);
            DatabaseService.SetContext(context);
        }

        [HttpGet]
        public IActionResult TransactionDetail(Guid transactionId)
        {
            var test = DatabaseService.GetTransactionDetails(transactionId);
            return PartialView("_TransactionDetail", test);
        }
        public IActionResult TransactionRegistrationpartial()
        {
            Dictionary<int, string> allStoreNames = DatabaseService.GetAllStores().ToDictionary(s => s.StoreId, s => s.Address);
            Dictionary<Guid, string> CustomerNames = DatabaseService.GetAllUsers().ToDictionary(u => u.UserId, u => u.Username);
            var allProductsInStore = DatabaseService.GetTotalProductInfo(1);

            ViewBag.AllStoreNames = allStoreNames;
            ViewBag.CustomerNames = CustomerNames;
            ViewBag.AllProductsInStore = allProductsInStore;
            ViewBag.AllProductsInStoreDict = allProductsInStore.ToDictionary(ps => ps.ProductId, ps => ps.Name);
            return PartialView("_TransactionRegistrationpartial");
        }
    }
}