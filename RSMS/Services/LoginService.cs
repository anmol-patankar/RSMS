using Domain.Models;
using RSMS.ViewModels;

namespace RSMS.Services
{
    public static class LoginService
    {
        private static RsmsTestContext _context;
        public static void SetDBContext(RsmsTestContext context) { _context = context; }
        public static bool LoginUser(LoginModel login, byte[] aesKey)
        {
            var loginUser = _context.UserInfos.FirstOrDefault(user => user.Username == login.Username);
            if (loginUser != null)
                if (loginUser.PasswordHashed.SequenceEqual(PasswordHandlerService.VerifyPassword(login.Password, aesKey, loginUser.Salt)))
                    return true;
            return false;
        }
    }
}
