using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class ProductInfo
{
    public Guid ProductId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int PriceBeforeTax { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
