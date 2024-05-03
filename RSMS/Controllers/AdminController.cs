using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.ActionAttributes;
using RSMS.Services;
using RSMS.ViewModels;

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
        public IActionResult Dashboard()
        {
            List<UserInfo> allUsers = DatabaseService.GetAllUsers();
            List<Store> allStores = DatabaseService.GetAllStores();
            List<UserRoleView> allRoleMaps = new();
            foreach (var user in allUsers)
            {
                var currentUserRoles = DatabaseService.GetRolesOfUser(user.Username);
                foreach (var currentUser in currentUserRoles)
                {
                    allRoleMaps.Add(new UserRoleView { UserId = user.UserId, Username = user.Username, RoleName = currentUser.RoleName });
                }
            }

            ViewData["User"] = allUsers;
            ViewData["Store"] = allStores;
            ViewData["RoleMap"] = allRoleMaps;
            return View();
        }
        [HttpPost]

        public IActionResult DeleteUser(Guid userId)
        {
            DatabaseService.DeleteUser(userId);
            return RedirectToAction("Dashboard", "Admin");
        }

    }
}