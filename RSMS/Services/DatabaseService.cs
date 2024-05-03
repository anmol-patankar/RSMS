using Domain.Models;
using RSMS.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace RSMS.Services
{
    public static class DatabaseService
    {
        public static RsmsTestContext Context { get; set; }

        public static void AddUserAndRole(UserInfo userInfo, RoleMap roleMap)
        {
            Context.UserInfos.Add(userInfo);
            Context.RoleMaps.Add(roleMap);
            Context.SaveChanges();
        }

        public static bool DeleteUser(Guid userId)
        {
            var userToDelete = GetUser(userId);
            //bool isSuccessful=false;
            var currentUserRoles = GetRolesOfUser(userToDelete.Username);
            Context.RoleMaps.RemoveRange(currentUserRoles);
            Context.UserInfos.Remove(userToDelete);
            Context.SaveChanges();
            return true;
        }

        public static List<Store> GetAllStores()
        {
            return Context.Stores.ToList();
        }

        public static List<UserInfo> GetAllUsers()
        {
            return Context.UserInfos.ToList();
        }

        public static List<RoleMap> GetRolesOfUser(string username) => (from rolemap in Context.RoleMaps
                                                                        where rolemap.UserId == (from userInfo in Context.UserInfos
                                                                                                 where userInfo.Username == username
                                                                                                 select userInfo.UserId).First()
                                                                        select rolemap
                    ).ToList();

        public static UserInfo GetUser(Guid userId)
        {
            return Context.UserInfos.FirstOrDefault(u => u.UserId == userId);
        }

        public static UserInfo GetUser(string userNameOrEmail)
        {
            return Context.UserInfos.FirstOrDefault(u => u.Username == userNameOrEmail || u.Email == userNameOrEmail);
        }

        public static List<ValidationResult> IsDuplicateUserRegistration(UserRegistrationModel user)
        {
            List<ValidationResult> errors = [];
            if (GetUser(user.Username) != null)
                errors.Add(new ValidationResult("Username already exists", ["Username"]));
            if (GetUser(user.Email) != null)
                errors.Add(new ValidationResult("Email is already registered", ["Email"]));
            return errors;
        }

        public static bool LoginCredentialValidity(string username, string password)
        {
            var loginUserInDatabase = GetUser(username);
            if (loginUserInDatabase != null && loginUserInDatabase.PasswordHashed.SequenceEqual(SecurityService.HashStringWithSalt(password ?? "", loginUserInDatabase.Salt)))
                return true;
            return false;
        }

        public static void SetContext(RsmsTestContext context)
        { Context = context; }
    }
}