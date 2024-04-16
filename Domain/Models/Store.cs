using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string Name { get; set; } = null!;

    public int Rent { get; set; }

    public virtual ICollection<Paydesk> Paydesks { get; set; } = new List<Paydesk>();

    public virtual ICollection<PayrollHistory> PayrollHistories { get; set; } = new List<PayrollHistory>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<UserInfo> UserInfos { get; set; } = new List<UserInfo>();
}
