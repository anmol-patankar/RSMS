using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.Services;
using RSMS.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace RSMS.Controllers
{
    public class PayrollController : Controller
    {
        public PayrollController(RsmsTestContext context, IConfiguration config)
        {
            SecurityService.SetKeyConfig(config[Constants.AesKeyString], config);
            DatabaseService.SetContext(context);
        }
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet]
        public IActionResult GetPayrollHistory(Guid userId, int storeId)
        {
            List<PayrollHistory> payrollList = null;
            if (HttpContext.User.IsInRole("Admin"))
            {
                payrollList = DatabaseService.GetSalaryHistory();
            }
            else if (HttpContext.User.IsInRole("Manager"))
            {
                payrollList = DatabaseService.GetSalaryHistory(storeId: storeId);
            }
            else if (HttpContext.User.IsInRole("Employee"))
            {
                payrollList = DatabaseService.GetSalaryHistory(userId: userId);
            }
            return PartialView("_PayrollViewPartial", payrollList);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]

        public IActionResult PayrollRegistrationPartial()
        {
            Dictionary<Guid, string> employeeNames;
            if (HttpContext.User.IsInRole("Admin"))
            {
                employeeNames = DatabaseService.GetAllUsers().Where(u => u.RoleId >= 4).ToDictionary(u => u.UserId, u => u.Username);

            }
            else
            {
                employeeNames = DatabaseService.GetAllUsers()
                    .Where(u => u.RoleId == 4 && u.StoreId == DatabaseService.GetUser(HttpContext.User.Identity.Name).StoreId)
                    .ToDictionary(u => u.UserId, u => u.Username);

            }
            ViewBag.EmployeeNames = employeeNames;
            return PartialView("_PayrollRegistrationPartial");
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public IActionResult PayrollRegistration(PayrollRegistrationModel payrollEntry)
        {
            ModelState.Clear();

            if (ModelState.IsValid)
            {
                payrollEntry.PayrollId = Guid.NewGuid();
                payrollEntry.AuthorizerId = DatabaseService.GetUser(HttpContext.User.Identity.Name).UserId;
                payrollEntry.StoreId = (int)DatabaseService.GetUser(payrollEntry.PayeeId).StoreId;
                payrollEntry.TransactionTime = DateTime.Now;
                payrollEntry.TaxDeduction = 5;
                DatabaseService.AddNewPayroll((PayrollHistory)payrollEntry);
                return Json(new { success = true });
            }
            return PartialView("_PayrollRegistrationPartial", payrollEntry);

        }
    }
}