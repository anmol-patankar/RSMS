using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.ActionAttributes;
using RSMS.Services;
using RSMS.ViewModels;
using System.Linq;
using System.Text.RegularExpressions;

namespace RSMS.Controllers
{
    public class CommonController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult GetStores()
        {
            return View(DatabaseService.GetAllStores());
        }
        //public IActionResult GetTransactions()
        //{

        //}
        //public IActionResult GetProducts()
        //{

        //}
    }
}
