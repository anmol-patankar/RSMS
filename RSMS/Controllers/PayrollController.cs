using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using RSMS.Services;

namespace RSMS.Controllers
{
    public class PayrollController : Controller
    {
        public PayrollController(RsmsTestContext context, IConfiguration config)
        {
            SecurityService.SetKeyConfig(config[Constants.AesKeyString], config);
            DatabaseService.SetContext(context);
        }

        [HttpGet]
        public IActionResult GetPayrollHistory(Guid userId, int storeId)
        {
            List<PayrollHistory> payrollList = null;
            if (HttpContext.User.IsInRole("Admin") || HttpContext.User.IsInRole("Manager"))
            {
                payrollList = DatabaseService.GetSalaryHistory();
            }
            else if (HttpContext.User.IsInRole("Employee"))
            {
                payrollList = DatabaseService.GetSalaryHistory(storeId: storeId);
            }
            else
            {
                payrollList = DatabaseService.GetSalaryHistory(userId: userId);
            }

            return PartialView("_PayrollViewPartial", payrollList);
        }
    }
}