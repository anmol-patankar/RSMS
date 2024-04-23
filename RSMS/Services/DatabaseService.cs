﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using RSMS.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RSMS.Services
{
    public static class DatabaseService
    {
        public static RsmsTestContext Context { get; set; }
        public static void SetContext(RsmsTestContext context) { Context = context; }
        public static List<ValidationResult> IsDuplicateUserRegistration(UserRegistrationModel user)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
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
        public static bool LoginUser(UserLoginModel login)
        {
            var loginUserInDatabase = Context.UserInfos.FirstOrDefault(user => user.Username == login.Username);
            if (loginUserInDatabase != null && loginUserInDatabase.PasswordHashed.SequenceEqual(EncryptionService.VerifyPassword(login.Password, loginUserInDatabase.Salt)))
                return true;
            return false;
        }
    }
}
