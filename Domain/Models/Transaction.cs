using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Transaction
{
    public Guid TransactionId { get; set; }

    public int StoreId { get; set; }

    public Guid CashierId { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime TransactionTimestamp { get; set; }

    public virtual UserInfo Cashier { get; set; } = null!;

    public virtual UserInfo Customer { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
}
