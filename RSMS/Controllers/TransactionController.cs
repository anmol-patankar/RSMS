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
    }
}