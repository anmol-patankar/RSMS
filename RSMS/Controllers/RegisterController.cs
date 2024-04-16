using Microsoft.AspNetCore.Mvc;
using System.Text;
using RSMS.ViewModels;
using Domain.Models;
using RSMS.Services;
public class RegisterController : Controller
{
    private readonly RsmsTestContext context;
    private readonly IConfiguration _config;

    public RegisterController(RsmsTestContext context, IConfiguration config)
    {
        this.context = context;
        _config = config;
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Register(UserRegistrationModel user)
    {
        byte[] aesKey = Encoding.UTF8.GetBytes(_config["AesEncryption:Key"]);
        if (ModelState.IsValid)
        {
            RegistrationService.RegisterUser(user, aesKey, out UserInfo userInfo);
            context.UserInfos.Add(userInfo);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        else
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
            return View(user);
        }
    }
}
