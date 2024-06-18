using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.Services;
using RSMS.ViewModels;

namespace RSMS.Controllers
{
    public class TransactionController : Controller
    {
        public TransactionController(RsmsTestContext context, IConfiguration config)
        {
            SecurityService.SetKeyConfig(config[Constants.AesKeyString], config);
            DatabaseService.SetContext(context);
        }
        [Authorize]
        [HttpGet]
        public IActionResult TransactionDetail(Guid transactionId)
        {
            var test = DatabaseService.GetTransactionDetails(transactionId);
            return PartialView("_TransactionDetail", test);
        }
        [Authorize(Roles = "Admin,Manager,Employee")]
        public IActionResult TransactionRegistrationPartial()
        {
            Dictionary<int, string> allStoreNames = DatabaseService.GetAllStores().ToDictionary(s => s.StoreId, s => s.Address);
            Dictionary<Guid, string> customerNames = DatabaseService.GetAllUsers().ToDictionary(u => u.UserId, u => u.Username);
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
            ViewBag.CustomerNames = customerNames;
            ViewBag.AllProductsInStore = allProductsInStore;
            ViewBag.AllProductsInStoreDict = allProductsInStore.ToDictionary(ps => ps.ProductId, ps => ps.Name);
            return PartialView("_TransactionRegistrationPartial");
        }
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpPost]
        public IActionResult RegisterTransaction(TotalTransactionModel totalTransaction)
        {
            if (!ModelState.IsValid)
            {
                totalTransaction.Transaction.TransactionId = Guid.NewGuid();
                totalTransaction.Transaction.TransactionTimestamp = DateTime.Now;
                foreach (var transactionProduct in totalTransaction.TransactionDetailList)
                {
                    transactionProduct.TransactionId = totalTransaction.Transaction.TransactionId;

                }
                DatabaseService.AddNewTransaction(totalTransaction.Transaction);
                DatabaseService.AddTransactionDetails(totalTransaction.TransactionDetailList);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}