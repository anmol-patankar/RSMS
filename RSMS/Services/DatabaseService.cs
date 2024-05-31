using Domain.Models;
using Microsoft.CodeAnalysis;
using RSMS.ViewModels;
using System.Collections.Generic;
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
        public enum TaxRate
        {
            GeneralSalesTax = 1,
            LuxuryGoodsTax = 2,
            FoodAndBeverageTax = 3,
            ElectronicsTax = 4,
            EssentialGoodsTax = 5
        }
        public static List<StoreAvailibilityModel> GetProductAvailibility(string productId)
        {
            List<StoreAvailibilityModel> storeAvailibility = new();
            var productPriceBeforeTax = Context.ProductInfos.Where(pi => pi.ProductId == productId).First().PriceBeforeTax;
            var storeStockOfProduct = Context.ProductStocks.Where(ps => ps.ProductId == productId).ToList();
            foreach (var storeStock in storeStockOfProduct)
            {
                storeAvailibility.Add(new StoreAvailibilityModel
                {
                    StoreId = storeStock.StoreId,
                    DiscountPercent = storeStock.DiscountPercent,
                    Quantity = storeStock.Quantity
                });
            }
            return storeAvailibility.OrderBy(s => s.StoreId).ToList();
        }
        public static void AddProductStockToStore(ProductStock stock)
        {
            Context.ProductStocks.Add(stock);
            Context.SaveChanges();
        }
        public static List<string> GetNotAddedProducts(int storeId)
        {

            return (from p in Context.ProductInfos
                    where !(from ps in Context.ProductStocks
                            where ps.StoreId == storeId
                            select ps.ProductId).Contains(p.ProductId)
                    select p.Name).ToList();
        }
        public static List<TransactionDetail> GetTransactionDetails(Guid transactionId)
        {
            return Context.TransactionDetails.Where(td => td.TransactionId == transactionId).ToList();
        }
        public static List<Transaction> GetAllTransactions(Guid? userId = null)
        {
            if (userId == null)
            {
                return Context.Transactions.ToList();
            }
            else
            {

                return Context.Transactions.Where(t => t.CustomerId == userId).ToList();
            }
        }

        public static List<ProductInfo> GetAllProductInfos()
        {
            return Context.ProductInfos.ToList();
        }
        public static ProductInfo GetProductInfoFromID(string productId)
        {
            return Context.ProductInfos.Where(p => p.ProductId == productId).First();
        }
        public static ProductInfo GetProductInfoFromName(string name)
        {
            return Context.ProductInfos.Where(p => p.Name == name).First();
        }
        public static bool EditProductInfo(ProductInfo productInfo)
        {
            var existingProductInfo = Context.ProductInfos.First(pi => pi.ProductId == productInfo.ProductId);
            existingProductInfo.Name = productInfo.Name;
            existingProductInfo.Description = productInfo.Description;
            existingProductInfo.PriceBeforeTax = productInfo.PriceBeforeTax;
            existingProductInfo.Photo = productInfo.Photo;
            existingProductInfo.TaxType = productInfo.TaxType;

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