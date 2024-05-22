using Domain.Models;
using Microsoft.CodeAnalysis;
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
        public static ProductInfo GetProductInfo(string productId)
        {
            return Context.ProductInfos.Where(p => p.ProductId == productId).First();
        }
        public static bool EditProductInfo(ProductInfo productInfo)
        {
            var existingProductInfo = Context.ProductInfos.First(pi => pi.ProductId == productInfo.ProductId);
            existingProductInfo.Name = productInfo.Name;
            existingProductInfo.Description = productInfo.Description;
            existingProductInfo.PriceBeforeTax = productInfo.PriceBeforeTax;
            existingProductInfo.Photo = productInfo.Photo;

            Context.SaveChanges();
            return true;

        }
        public static List<TotalProductInfoModel> GetTotalProductInfo(int storeId)
        {
            var productsInStore = Context.ProductStocks.Where(p => p.StoreId == storeId).ToList();
            List<TotalProductInfoModel> totalProductInfo = new();
            foreach (var product in productsInStore)
            {
                var productInfo = Context.ProductInfos.Where(p => p.ProductId == product.ProductId).First();
                totalProductInfo.Add(new TotalProductInfoModel
                {
                    ProductId = product.ProductId,
                    Description = productInfo.Description,
                    DiscountPercent = product.DiscountPercent,
                    Name = productInfo.Name,
                    PriceBeforeTax = productInfo.PriceBeforeTax,
                    Quantity = product.Quantity,
                    Photo = productInfo.Photo
                });
            }
            return totalProductInfo;
        }

        public static void UpdateProductStock(int storeId, string productId, int quantity)
        {
            Context.ProductStocks.First(ps => ps.StoreId == storeId && ps.ProductId == productId).Quantity = quantity;
            Context.SaveChanges();

        }
        public static void UpdateProductDiscount(int storeId, string productId, int discountPercent)
        {
            Context.ProductStocks.First(ps => ps.StoreId == storeId && ps.ProductId == productId).DiscountPercent = discountPercent;
            Context.SaveChanges();

        }
        public static int RoleNameToRoleId(string roleName)
        {
            return (int)(UserRole)Enum.Parse(typeof(UserRole), roleName);
        }

        public static RsmsTestContext Context { get; set; }

        public static void AddUser(UserInfo userInfo)
        {
            Context.UserInfos.Add(userInfo);
            Context.SaveChanges();
        }

        public static void SetRole(Guid userId, string roleToSet)
        {
            var updatedUser = Context.UserInfos.Where(u => u.UserId == userId).First().RoleId = RoleNameToRoleId(roleToSet);
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

        public static bool DeleteStore(int storeId)
        {
            var storeToDelete = GetStore(storeId);
            Context.Stores.Remove(storeToDelete);
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

        public static Store GetStore(int storeId)
        {
            return Context.Stores.First(s => s.StoreId == storeId);
        }

        public static void EditStore(Store store)
        {
            Context.Stores.Update(store);
            Context.SaveChanges();
        }

        public static string? GetRoleOfUser(string username)
        {
            return (Enum.GetName(typeof(UserRole), (from userinfo in Context.UserInfos where userinfo.Username == username select userinfo.RoleId).First()));
        }

        public static string? GetRoleOfUser(Guid userid)
        {
            return (Enum.GetName(typeof(UserRole), (from userinfo in Context.UserInfos where userinfo.UserId == userid select userinfo.RoleId).First()));
        }

        public static UserInfo? GetUser(Guid userId)
        {
            return Context.UserInfos.FirstOrDefault(u => u.UserId == userId);
        }

        public static UserInfo? GetUser(string userNameOrEmail)
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