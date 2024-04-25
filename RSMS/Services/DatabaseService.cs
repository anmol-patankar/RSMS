using Domain.Models;
using Microsoft.EntityFrameworkCore;
using RSMS.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace RSMS.Services
{
    public static class DatabaseService
    {
        public static RsmsTestContext Context { get; set; }
        public static void SetContext(RsmsTestContext context) { Context = context; }
        public static List<string> GetRolesOfUser(string username) => (from rolemap in Context.RoleMaps
                                                                       where rolemap.UserId == (from userInfo in Context.UserInfos
                                                                                                where userInfo.Username == username
                                                                                                select userInfo.UserId).First()
                                                                       select rolemap.RoleName
                    ).ToList();
        public static List<ValidationResult> IsDuplicateUserRegistration(UserRegistrationModel user)
        {
            List<ValidationResult> errors = [];
            if (Context.UserInfos.FirstOrDefault(u => u.Username == user.Username) != null)
                errors.Add(new ValidationResult("Username already exists", ["Username"]));
            if (Context.UserInfos.FirstOrDefault(u => u.Email == user.Email) != null)
                errors.Add(new ValidationResult("Email is already registered", ["Email"]));
            return errors;
        }
        /// <summary>
        /// Adds user, and also updates RoleMap
        /// </summary>
        /// <param name="userInfo"></param>
        public static void AddUserAndRole(UserInfo userInfo, RoleMap roleMap)
        {
            Context.UserInfos.Add(userInfo);
            Context.RoleMaps.Add(roleMap);
            Context.SaveChanges();

        }
        public static bool LoginCredentialValidity(UserLoginModel login)
        {
            var loginUserInDatabase = Context.UserInfos.FirstOrDefault(user => user.Username == login.Username);
            if (loginUserInDatabase != null && loginUserInDatabase.PasswordHashed.SequenceEqual(SecurityService.HashStringWithSalt(login.Password ?? "", loginUserInDatabase.Salt)))
                return true;
            return false;
        }
        public static UserInfo GetUser(Guid userId)
        {
            return Context.UserInfos.FirstOrDefault(u => u.UserId == userId) ?? new UserInfo();
        }

        public static UserInfo GetUser(string userNameOrEmail)
        {
            return Context.UserInfos.FirstOrDefault(u => u.Username == userNameOrEmail || u.Email == userNameOrEmail) ?? new UserInfo();
        }
    }
}
