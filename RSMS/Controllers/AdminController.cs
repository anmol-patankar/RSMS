using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSMS.Services;
using RSMS.ViewModels;
using System.Text;

namespace RSMS.Controllers
{
    public class AdminController : Controller
    {

        public AdminController(RsmsTestContext context, IConfiguration config)
        {
            //_context = context;
            //_config = config;
            SecurityService.SetKeyConfig((config["AesEncryption:Key"]), config);
            DatabaseService.SetContext(context);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            List<UserInfo> allUsers = DatabaseService.GetAllUsers();
            List<Store> allStores = DatabaseService.GetAllStores();
            List<UserRoleView> allRoleMaps = new();
            foreach (var user in allUsers)
            {
                var currentUserRoles = DatabaseService.GetRolesOfUser(user.Username);
                foreach (var currentUser in currentUserRoles)
                {
                    allRoleMaps.Add(new UserRoleView { userId = user.UserId, Username = user.Username, roleName = currentUser.RoleName });
                }
            }

            ViewData["User"] = allUsers;
            ViewData["Store"] = allStores;
            ViewData["RoleMap"] = allRoleMaps;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(Guid userId)
        {
            //if (DatabaseService.DeleteUser(userId))
            //{
            //    return Json(new { success = true, message = "Deleted Successfully" });
            //}
            //else
            //{
            //    return Json(new { success = false, message = "Deletion failed" });

            //}
            DatabaseService.DeleteUser(userId);
            return RedirectToAction("AdminDashboard", "Admin");
        }

    }
}