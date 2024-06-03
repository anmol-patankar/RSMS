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
            entity.HasKey(e => e.PayrollId).HasName("PK__Payroll___D99FC944096DD3AD");

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
                .HasConstraintName("FK__Payroll_H__autho__4222D4EF");

            entity.HasOne(d => d.Payee).WithMany(p => p.PayrollHistoryPayees)
                .HasForeignKey(d => d.PayeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payroll_H__tax_d__412EB0B6");

            entity.HasOne(d => d.Store).WithMany(p => p.PayrollHistories)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payroll_H__store__4316F928");
        });

        modelBuilder.Entity<ProductInfo>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product___47027DF5D121D147");

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
            entity.Property(e => e.TaxType)
                .HasDefaultValue(0)
                .HasColumnName("tax_type");

            entity.HasOne(d => d.TaxTypeNavigation).WithMany(p => p.ProductInfos)
                .HasForeignKey(d => d.TaxType)
                .HasConstraintName("FK__Product_I__tax_t__48CFD27E");
        });

        modelBuilder.Entity<ProductStock>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.ProductId }).HasName("PK__Product___E68284D3256808CF");

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
                .HasConstraintName("FK__Product_S__produ__4BAC3F29");

            entity.HasOne(d => d.Store).WithMany(p => p.ProductStocks)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product_S__store__4CA06362");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK__Store__A2F2A30CC7FE42C4");

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
            entity.HasKey(e => e.TaxType).HasName("PK__Tax_Rate__F8B6202EB2E1EB61");

            entity.ToTable("Tax_Rates");

            entity.Property(e => e.TaxType)
                .ValueGeneratedNever()
                .HasColumnName("tax_type");
            entity.Property(e => e.TaxRate1).HasColumnName("tax_rate");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__85C600AFA8824544");

            entity.Property(e => e.TransactionId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("transaction_id");
            entity.Property(e => e.CashierId).HasColumnName("cashier_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.TransactionTimestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("transaction_timestamp");

            entity.HasOne(d => d.Cashier).WithMany(p => p.TransactionCashiers)
                .HasForeignKey(d => d.CashierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__cashi__52593CB8");

            entity.HasOne(d => d.Customer).WithMany(p => p.TransactionCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__custo__534D60F1");

            entity.HasOne(d => d.Store).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__store__5165187F");
        });

        modelBuilder.Entity<TransactionDetail>(entity =>
        {
            entity.HasKey(e => new { e.TransactionId, e.ProductId }).HasName("PK__Transact__C1B627703CF0E30A");

            entity.ToTable("Transaction_Details");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.ProductId)
                .HasMaxLength(5)
                .HasColumnName("product_id");
            entity.Property(e => e.DiscountPercent).HasColumnName("discount_percent");
            entity.Property(e => e.PriceBeforeTax).HasColumnName("price_before_tax");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");
            entity.Property(e => e.TaxPercent).HasColumnName("tax_percent");

            entity.HasOne(d => d.Product).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__produ__571DF1D5");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__trans__5629CD9C");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User_Inf__B9BE370FB6B17923");

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