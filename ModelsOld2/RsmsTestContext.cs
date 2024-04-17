using System;
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

    public virtual DbSet<Paydesk> Paydesks { get; set; }

    public virtual DbSet<PayrollHistory> PayrollHistories { get; set; }

    public virtual DbSet<ProductInfo> ProductInfos { get; set; }

    public virtual DbSet<ProductStock> ProductStocks { get; set; }

    public virtual DbSet<ProductTaxis> ProductTaxes { get; set; }

    public virtual DbSet<RoleMap> RoleMaps { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<TaxRate> TaxRates { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionDetail> TransactionDetails { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=RSMS_Test;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Paydesk>(entity =>
        {
            entity.HasKey(e => e.PaydeskNumber).HasName("PK__Paydesk__BC17994A40760AA7");

            entity.ToTable("Paydesk");

            entity.Property(e => e.PaydeskNumber)
                .ValueGeneratedNever()
                .HasColumnName("paydesk_number");
            entity.Property(e => e.IsManned).HasColumnName("is_manned");
            entity.Property(e => e.StoreId).HasColumnName("store_id");

            entity.HasOne(d => d.Store).WithMany(p => p.Paydesks)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK__Paydesk__store_i__3F466844");
        });

        modelBuilder.Entity<PayrollHistory>(entity =>
        {
            entity.HasKey(e => e.PayrollId).HasName("PK__Payroll___D99FC94425D6FB4E");

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
                .HasConstraintName("FK__Payroll_H__autho__440B1D61");

            entity.HasOne(d => d.Payee).WithMany(p => p.PayrollHistoryPayees)
                .HasForeignKey(d => d.PayeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payroll_H__tax_d__4316F928");

            entity.HasOne(d => d.Store).WithMany(p => p.PayrollHistories)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payroll_H__store__44FF419A");
        });

        modelBuilder.Entity<ProductInfo>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product___47027DF5F22FD2C5");

            entity.ToTable("Product_Info");

            entity.HasIndex(e => e.ProductCode, "UQ__Product___AE1A8CC424A16C60").IsUnique();

            entity.Property(e => e.ProductId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("product_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.PriceBeforeTax).HasColumnName("price_before_tax");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(5)
                .HasColumnName("product_code");
        });

        modelBuilder.Entity<ProductStock>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Product_Stock");

            entity.Property(e => e.DiscountPercent).HasColumnName("discount_percent");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.StoreId).HasColumnName("store_id");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product_S__produ__4AB81AF0");

            entity.HasOne(d => d.Store).WithMany()
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product_S__store__4BAC3F29");
        });

        modelBuilder.Entity<ProductTaxis>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Product_Taxes");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.TaxType).HasColumnName("tax_type");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product_T__produ__4F7CD00D");

            entity.HasOne(d => d.TaxTypeNavigation).WithMany()
                .HasForeignKey(d => d.TaxType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product_T__tax_t__5070F446");
        });

        modelBuilder.Entity<RoleMap>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Role_Map");

            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .HasColumnName("role_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Role_Map__user_i__3C69FB99");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK__Store__A2F2A30CC0EF376C");

            entity.ToTable("Store");

            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<TaxRate>(entity =>
        {
            entity.HasKey(e => e.TaxType).HasName("PK__Tax_Rate__F8B6202E5C4C9895");

            entity.ToTable("Tax_Rate");

            entity.Property(e => e.TaxType)
                .ValueGeneratedNever()
                .HasColumnName("tax_type");
            entity.Property(e => e.TaxRate1).HasColumnName("tax_rate");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__85C600AF01B4CDA5");

            entity.Property(e => e.TransactionId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("transaction_id");
            entity.Property(e => e.CashierId).HasColumnName("cashier_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.PaydeskNumber).HasColumnName("paydesk_number");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.StoreId).HasColumnName("store_id");

            entity.HasOne(d => d.Cashier).WithMany(p => p.TransactionCashiers)
                .HasForeignKey(d => d.CashierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__cashi__5629CD9C");

            entity.HasOne(d => d.Customer).WithMany(p => p.TransactionCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__custo__571DF1D5");

            entity.HasOne(d => d.PaydeskNumberNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.PaydeskNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__payde__5812160E");

            entity.HasOne(d => d.Product).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__produ__5441852A");

            entity.HasOne(d => d.Store).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__store__5535A963");
        });

        modelBuilder.Entity<TransactionDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Transaction_Details");

            entity.Property(e => e.DiscountAmount).HasColumnName("discount_amount");
            entity.Property(e => e.PriceBeforeTax).HasColumnName("price_before_tax");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.TaxAmount).HasColumnName("tax_amount");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__produ__5AEE82B9");

            entity.HasOne(d => d.Transaction).WithMany()
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__trans__59FA5E80");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User_Inf__B9BE370F1618F867");

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
            entity.Property(e => e.Salt)
                .HasMaxLength(256)
                .HasColumnName("salt");
            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Store).WithMany(p => p.UserInfos)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK__User_Info__regis__3A81B327");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
