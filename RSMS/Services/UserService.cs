using Domain.Models;
using Microsoft.EntityFrameworkCore;
using RSMS.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace RSMS.Services
{
    public class UserService
    {
        private static RsmsTestContext _context;
        public static void SetContextAndKey(RsmsTestContext context, byte[] aesKey)
        {
            _context = context;
            aes.Key = aesKey;
        }
        private static List<ValidationResult> _errors = new List<ValidationResult>();
        public static List<ValidationResult> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }
        private static Aes aes = Aes.Create();
        public static void RegisterUser(UserRegistrationModel user, UserRoles userRole)
        {

            Errors.Clear();
            var userGUID = Guid.NewGuid();
            byte[] aesIV;

            if (_context.UserInfos.FirstOrDefault(u => u.Username == user.Username) != null)
                Errors.Add(new ValidationResult("Username already exists", ["Username"]));
            if (_context.UserInfos.FirstOrDefault(u => u.Email == user.Email) != null)
                Errors.Add(new ValidationResult("Email is already regsitered", ["Email"]));
            if (Errors.Count > 0) return;
            var passwordHashed = HashPassword(user.Password, out aesIV);
            var userInfo = new UserInfo()
            {
                UserId = userGUID,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Dob = user.Dob,
                Email = user.Email,
                Salt = aesIV,
                PasswordHashed = passwordHashed,
                Phone = user.Phone,
                RegistrationDate = DateTime.Now
            };
            if (userRole == UserRoles.Customer)
                userInfo.StoreId = 0;
            RoleMap roleMap = new RoleMap()
            {
                UserId = userGUID,
                RoleName = userRole.ToString()
            };

            _context.UserInfos.Add(userInfo);
            _context.RoleMaps.Add(roleMap);
            _context.SaveChanges();
        }
        public static bool LoginUser(LoginModel login)
        {
            var loginUser = _context.UserInfos.FirstOrDefault(user => user.Username == login.Username);
            if (loginUser != null)
                if (loginUser.PasswordHashed.SequenceEqual(VerifyPassword(login.Password, loginUser.Salt)))
                    return true;
            return false;
        }
        private static byte[] PasswordEncryptor(string password)
        {
            using (var encryptor = aes.CreateEncryptor())
            {
                byte[] saltedPassword = Encoding.UTF8.GetBytes(password);
                return encryptor.TransformFinalBlock(saltedPassword, 0, saltedPassword.Length);
            }
        }
        public static byte[] VerifyPassword(string password, byte[] salt)
        {
            aes.IV = salt;
            return PasswordEncryptor(password);
        }
        public static byte[] HashPassword(string password, out byte[] aesIV)
        {
            aes.GenerateIV();
            aesIV = aes.IV;
            return PasswordEncryptor(password);
        }
    }
}
