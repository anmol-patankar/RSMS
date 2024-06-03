namespace Domain.Models;

public partial class Paydesk
{
    public int PaydeskNumber { get; set; }

    public int? StoreId { get; set; }

    public bool? IsManned { get; set; }

    public virtual Store? Store { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}