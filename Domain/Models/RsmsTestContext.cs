﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public partial class RsmsTestContext : DbContext
{
    public RsmsTestContext()
    {
    }

    public RsmsTestContext(DbContextOptions<RsmsTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PayrollHistory> PayrollHistories { get; set; }

    public virtual DbSet<ProductInfo> ProductInfos { get; set; }

    public virtual DbSet<ProductStock> ProductStocks { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<TaxRate> TaxRates { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionDetail> TransactionDetails { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:dbcs");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PayrollHistory>(entity =>
        {
            entity.HasKey(e => e.PayrollId).HasName("PK__Payroll___D99FC9441F3AEFDD");

            entity.ToTable("Payroll_History");

            entity.Property(e => e.PayrollId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("payroll_id");
            entity.Property(e => e.AuthorizerId).HasColumnName("authorizer_id");
            entity.Property(e => e.BaseAmount).HasColumnName("base_amount");
            entity.Property(e => e.PayeeId).HasColumnName("payee_id");
            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.TaxDeduction).HasColumnName("tax_deduction");
            entity.Property(e => e.TransactionTime)
                .HasColumnType("datetime")
                .HasColumnName("transaction_time");

            entity.HasOne(d => d.Authorizer).WithMany(p => p.PayrollHistoryAuthorizers)
                .HasForeignKey(d => d.AuthorizerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payroll_H__autho__44FF419A");

            entity.HasOne(d => d.Payee).WithMany(p => p.PayrollHistoryPayees)
                .HasForeignKey(d => d.PayeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payroll_H__tax_d__440B1D61");

            entity.HasOne(d => d.Store).WithMany(p => p.PayrollHistories)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payroll_H__store__45F365D3");
        });

        modelBuilder.Entity<ProductInfo>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product___47027DF592827A83");

            entity.ToTable("Product_Info");

            entity.Property(e => e.ProductId)
                .HasMaxLength(5)
                .HasColumnName("product_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Photo)
                .HasMaxLength(256)
                .HasColumnName("photo");
            entity.Property(e => e.PriceBeforeTax).HasColumnName("price_before_tax");

            entity.HasMany(d => d.TaxTypes).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductTaxis",
                    r => r.HasOne<TaxRate>().WithMany()
                        .HasForeignKey("TaxType")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Product_T__tax_t__5165187F"),
                    l => l.HasOne<ProductInfo>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Product_T__produ__5070F446"),
                    j =>
                    {
                        j.HasKey("ProductId", "TaxType").HasName("PK__Product___B8891FF7AA71E4EC");
                        j.ToTable("Product_Taxes");
                        j.IndexerProperty<string>("ProductId")
                            .HasMaxLength(5)
                            .HasColumnName("product_id");
                        j.IndexerProperty<int>("TaxType").HasColumnName("tax_type");
                    });
        });

        modelBuilder.Entity<ProductStock>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.ProductId }).HasName("PK__Product___E68284D37F93EBF7");

            entity.ToTable("Product_Stock");

            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.ProductId)
                .HasMaxLength(5)
                .HasColumnName("product_id");
            entity.Property(e => e.DiscountPercent).HasColumnName("discount_percent");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductStocks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product_S__produ__4AB81AF0");

            entity.HasOne(d => d.Store).WithMany(p => p.ProductStocks)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product_S__store__4BAC3F29");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK__Store__A2F2A30C5977204F");

            entity.ToTable("Store");

            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsOpen)
                .HasDefaultValue(true)
                .HasColumnName("is_open");
            entity.Property(e => e.Rent).HasColumnName("rent");
        });

        modelBuilder.Entity<TaxRate>(entity =>
        {
            entity.HasKey(e => e.TaxType).HasName("PK__Tax_Rate__F8B6202E2FA26858");

            entity.ToTable("Tax_Rate");

            entity.Property(e => e.TaxType)
                .ValueGeneratedNever()
                .HasColumnName("tax_type");
            entity.Property(e => e.TaxRate1).HasColumnName("tax_rate");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__85C600AFCE01C7FC");

            entity.Property(e => e.TransactionId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("transaction_id");
            entity.Property(e => e.CashierId).HasColumnName("cashier_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.StoreId).HasColumnName("store_id");

            entity.HasOne(d => d.Cashier).WithMany(p => p.TransactionCashiers)
                .HasForeignKey(d => d.CashierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__cashi__114A936A");

            entity.HasOne(d => d.Customer).WithMany(p => p.TransactionCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__custo__123EB7A3");

            entity.HasOne(d => d.Store).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__store__10566F31");
        });

        modelBuilder.Entity<TransactionDetail>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__85C600AFBD05DCDE");

            entity.ToTable("Transaction_Details");

            entity.Property(e => e.TransactionId)
                .ValueGeneratedNever()
                .HasColumnName("transaction_id");
            entity.Property(e => e.DiscountAmount).HasColumnName("discount_amount");
            entity.Property(e => e.PriceBeforeTax).HasColumnName("price_before_tax");
            entity.Property(e => e.ProductId)
                .HasMaxLength(5)
                .HasColumnName("product_id");
            entity.Property(e => e.TaxAmount).HasColumnName("tax_amount");

            entity.HasOne(d => d.Product).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__produ__160F4887");

            entity.HasOne(d => d.Transaction).WithOne(p => p.TransactionDetail)
                .HasForeignKey<TransactionDetail>(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__trans__151B244E");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User_Inf__B9BE370F0A9E598F");

            entity.ToTable("User_Info");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("user_id");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.IsDisabled).HasColumnName("is_disabled");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.PasswordHashed)
                .HasMaxLength(256)
                .HasColumnName("password_hashed");
            entity.Property(e => e.Phone)
                .HasMaxLength(13)
                .HasColumnName("phone");
            entity.Property(e => e.RegistrationDate)
                .HasColumnType("datetime")
                .HasColumnName("registration_date");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Salt)
                .HasMaxLength(256)
                .HasColumnName("salt");
            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Store).WithMany(p => p.UserInfos)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK__User_Info__store__3D5E1FD2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
