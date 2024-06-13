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
            int storeIdOfCurrentUser = (int)DatabaseService.GetUser(HttpContext.User.Identity.Name).StoreId;
            string usernameOfCurrentUser = DatabaseService.GetUser(HttpContext.User.Identity.Name).Username;
            string userIdOfCurrentUser = DatabaseService.GetUser(HttpContext.User.Identity.Name).UserId.ToString();
            string storeNameOfCurrentUser = DatabaseService.GetStore(storeIdOfCurrentUser).Address;
            ViewBag.StoreId = storeIdOfCurrentUser;
            ViewBag.StoreName = storeNameOfCurrentUser;
            ViewBag.UsernameOfCurrentUser = usernameOfCurrentUser;
            ViewBag.UserIdOfCurrentUser = userIdOfCurrentUser;
            List<ViewModels.TotalProductInfoModel> allProductsInStore;
            if (HttpContext.User.IsInRole("Admin"))
            {
                allProductsInStore = DatabaseService.GetTotalProductInfo(storeId: 1);

            }
            else
            {
                allProductsInStore = DatabaseService.GetTotalProductInfo(storeId: storeIdOfCurrentUser);

            }
            ViewBag.CustomerNames = CustomerNames;
            ViewBag.AllProductsInStore = allProductsInStore;
            ViewBag.AllProductsInStoreDict = allProductsInStore.ToDictionary(ps => ps.ProductId, ps => ps.Name);
            return PartialView("_TransactionRegistrationpartial");
        }
    }
}