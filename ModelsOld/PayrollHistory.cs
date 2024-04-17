using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class PayrollHistory
{
    public Guid PayrollId { get; set; }

    public Guid PayeeId { get; set; }

    public Guid AuthorizerId { get; set; }

    public Guid StoreId { get; set; }

    public DateTime TransactionTime { get; set; }

    public int BaseAmount { get; set; }

    public int TaxDeduction { get; set; }

    public virtual UserInfo Authorizer { get; set; } = null!;

    public virtual UserInfo Payee { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
