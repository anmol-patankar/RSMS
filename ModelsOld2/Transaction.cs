using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Transaction
{
    public Guid TransactionId { get; set; }

    public Guid ProductId { get; set; }

    public int StoreId { get; set; }

    public Guid CashierId { get; set; }

    public Guid CustomerId { get; set; }

    public int PaydeskNumber { get; set; }

    public virtual UserInfo Cashier { get; set; } = null!;

    public virtual UserInfo Customer { get; set; } = null!;

    public virtual Paydesk PaydeskNumberNavigation { get; set; } = null!;

    public virtual ProductInfo Product { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
