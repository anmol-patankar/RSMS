using Domain.Models;
using RSMS.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace RSMS.Services
{
    public static class DatabaseService
    {
        public enum UserRole : int
        {
            Customer = 2,
            Employee = 4,
            Manager = 6,
            Admin = 8
        }

        public static RsmsTestContext Context { get; set; }

        public static void AddUser(UserInfo userInfo)
        {
            Context.UserInfos.Add(userInfo);
            Context.SaveChanges();
        }

        public static void SetRole(Guid userId, string rolesToSet)
        {
            var updatedUser = Context.UserInfos.Where(u => u.UserId == userId).First().RoleId = (int)(UserRole)Enum.Parse(typeof(UserRole), rolesToSet);
        }

        public static void EditUser(UserInfo userInfo)
        {
            Context.UserInfos.Update(userInfo);
            Context.SaveChanges();
        }

        public static bool DeleteUser(Guid userId)
        {
            var userToDelete = GetUser(userId);
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

        //        public static void EditRoles(List<string> rolesToAdd, List<string> rolesToRemove, Guid userId)
        //        {
        //            RemoveRoles(rolesToRemove, userId);
        //            Context.SaveChanges();
        //            AddRoles(rolesToAdd, userId);
        //            Context.SaveChanges();
        //        }

        //        public static void RemoveRoles(List<string> rolesToRemove, Guid userId)
        //        {
        //            using (var context = new RsmsTestContext())
        //            {
        //                foreach (var role in rolesToRemove)
        //                {
        //                    var roleToRemove = context.RoleMaps.FirstOrDefault(rm => rm.UserId == userId && rm.RoleName == role);
        //                    if (roleToRemove != null)
        //                    {
        //                        context.RoleMaps.Remove(roleToRemove);
        //                    }
        //                }
        //                context.SaveChanges();
        //            }
        //        }

        //        public static void AddRoles(List<string> rolesToAdd, Guid userId)
        //        {
        //            using (var context = new RsmsTestContext())
        //            {
        //                foreach (var role in rolesToAdd)
        //                {
        //                    var roleToAdd = context.RoleMaps.FirstOrDefault(rm => rm.UserId == userId && rm.RoleName == role);
        //                    if (roleToAdd == default(RoleMap))
        //                    {
        //                        context.RoleMaps.Add(new RoleMap() { UserId = userId, RoleName = role });
        //                    }
        //                }
        //                context.SaveChanges();
        //            }
        //        }

        public static string GetRoleOfUser(string username)
        {

            return (Enum.GetName(typeof(UserRole), (from userinfo in Context.UserInfos where userinfo.Username == username select userinfo.RoleId).First()));
        }
        public static string GetRoleOfUser(Guid userid)
        {
            return (Enum.GetName(typeof(UserRole), (from userinfo in Context.UserInfos where userinfo.UserId == userid select userinfo.RoleId).First()));
        }
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