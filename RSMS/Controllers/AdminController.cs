using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.ActionAttributes;
using RSMS.Services;
using RSMS.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static RSMS.Services.DatabaseService;

namespace RSMS.Controllers
{
    [Authorize(Roles = "Admin")]
    [NoCache]
    public class AdminController : Controller
    {
        public AdminController(RsmsTestContext context, IConfiguration config)
        {
            SecurityService.SetKeyConfig((config["AesEncryption:Key"]), config);
            DatabaseService.SetContext(context);
        }

        //public IActionResult Dashboard()
        //{

        //}


    }
}

