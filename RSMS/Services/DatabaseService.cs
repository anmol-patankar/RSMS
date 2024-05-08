using Domain.Models;
using RSMS.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace RSMS.Services
{
    public static class DatabaseService
    {
        public enum UserRoles
        {
            Customer, Manager, Admin, Employee
        }

        public static RsmsTestContext Context { get; set; }

        public static void AddUserAndRole(UserInfo userInfo, RoleMap roleMap)
        {
            Context.UserInfos.Add(userInfo);
            Context.RoleMaps.Add(roleMap);
            Context.SaveChanges();
        }
        public static void SetRoles(Guid userId, List<string> rolesToSet)
        {
            var tempRoleMap = new RoleMap();
            tempRoleMap.UserId = userId;
            List<string> availableRoles = ["Admin", "Manager", "Employee", "Customer"];
            foreach (var role in rolesToSet)
            {
                tempRoleMap.RoleName = role;
                availableRoles.Remove(role);
                try
                {
                    Context.RoleMaps.Add(tempRoleMap);
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }
        public static void EditUser(UserInfo userInfo)
        {
            Context.UserInfos.Update(userInfo);
            Context.SaveChanges();
        }

        public static bool DeleteUser(Guid userId)
        {
            var userToDelete = GetUser(userId);
            //bool isSuccessful=false;
            var currentUserRoles = GetRolesOfUser(userToDelete.Username);
            foreach (var currentRole in currentUserRoles)
            {
                RoleMap tempRoleMap = new() { UserId = userId, RoleName = currentRole };
                Context.RoleMaps.Remove(tempRoleMap);
            }
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
        public static void EditRoles(List<string> rolesToAdd, List<string> rolesToRemove, Guid userId)
        {
            RemoveRoles(rolesToRemove, userId);
            Context.SaveChanges();
            AddRoles(rolesToAdd, userId);
            Context.SaveChanges();
        }
        //public static void RemoveRoles(List<string> rolesToRemove, Guid userId)
        //{
        //    foreach (var role in rolesToRemove)
        //    {
        //        if (Context.RoleMaps.FirstOrDefault(rm => rm.UserId == userId && rm.RoleName == role) != null)
        //        {
        //            var tempRoleMap = new RoleMap() { UserId = userId, RoleName = role };
        //            Context.RoleMaps.Remove(tempRoleMap);
        //        }
        //        Context.SaveChanges();
        //    }
        //}
        //public static void AddRoles(List<string> rolesToAdd, Guid userId)
        //{

        //    foreach (var role in rolesToAdd)
        //    {

        //        if (Context.RoleMaps.FirstOrDefault(rm => rm.UserId == userId && rm.RoleName == role) == null)
        //        {
        //            var tempRoleMap = new RoleMap() { UserId = userId, RoleName = role };
        //            Context.RoleMaps.Add(tempRoleMap);
        //        }
        //        Context.SaveChanges();
        //    }

        //}
        public static void RemoveRoles(List<string> rolesToRemove, Guid userId)
        {
            using (var context = new RsmsTestContext())
            {
                foreach (var role in rolesToRemove)
                {
                    var roleToRemove = context.RoleMaps.FirstOrDefault(rm => rm.UserId == userId && rm.RoleName == role);
                    if (roleToRemove != null)
                    {
                        context.RoleMaps.Remove(roleToRemove);
                    }
                }
                context.SaveChanges();
            }
        }

        public static void AddRoles(List<string> rolesToAdd, Guid userId)
        {
            using (var context = new RsmsTestContext())
            {
                foreach (var role in rolesToAdd)
                {
                    var roleToAdd = context.RoleMaps.FirstOrDefault(rm => rm.UserId == userId && rm.RoleName == role);
                    if (roleToAdd == default(RoleMap))
                    {
                        context.RoleMaps.Add(new RoleMap() { UserId = userId, RoleName = role });
                    }
                }
                context.SaveChanges();
            }
        }

        public static List<string> GetRolesOfUser(string username) =>
          (from rolemap in Context.RoleMaps
           where rolemap.UserId == (from userInfo in Context.UserInfos
                                    where userInfo.Username == username
                                    select userInfo.UserId).First()
           select rolemap.RoleName
                       ).ToList();
        public static List<string> GetRolesOfUser(Guid userId) =>
          (from rolemap in Context.RoleMaps
           where rolemap.UserId == (from userInfo in Context.UserInfos
                                    where userInfo.UserId == userId
                                    select userInfo.UserId).First()
           select rolemap.RoleName
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