namespace Domain.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string Address { get; set; } = null!;

    public int Rent { get; set; }

    public bool IsOpen { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<PayrollHistory> PayrollHistories { get; set; } = new List<PayrollHistory>();

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<UserInfo> UserInfos { get; set; } = new List<UserInfo>();
}