using Domain.Models;
using RSMS.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace RSMS.Services
{
    public static class RegistrationService
    {
        private static RsmsTestContext _context;
        public static void SetDBContext(RsmsTestContext context) { _context = context; }
        private static List<ValidationResult> _errors = new List<ValidationResult>();
        public static List<ValidationResult> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        public static void RegisterUser(UserRegistrationModel user, byte[] aesKey, out UserInfo userInfo, UserRoles userRole)
        {
            var userGUID = Guid.NewGuid();
            byte[] aesIV;
            userInfo = null;

            if (_context.UserInfos.FirstOrDefault(u => u.Username == user.Username) != null)
                Errors.Add(new ValidationResult("Username already exists", ["Username"]));
            if (_context.UserInfos.FirstOrDefault(u => u.Email == user.Email) != null)
                Errors.Add(new ValidationResult("Email is already regsitered", ["Email"]));
            if (Errors.Count > 0) return;
            var passwordHashed = PasswordHandlerService.HashPassword(user.Password, aesKey, out aesIV);
            userInfo = new UserInfo()
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
            {
                userInfo.StoreId = 0;
            }
            RoleMap roleMap = new RoleMap()
            {
                UserId = userGUID,
                RoleName = userRole.ToString()
            };

            _context.UserInfos.Add(userInfo);
            _context.RoleMaps.Add(roleMap);
            _context.SaveChanges();
        }
    }
}

