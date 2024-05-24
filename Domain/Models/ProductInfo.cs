using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class ProductInfo
{
    public string ProductId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int PriceBeforeTax { get; set; }

    public string? Photo { get; set; }

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();

    public virtual ICollection<TaxRate> TaxTypes { get; set; } = new List<TaxRate>();
}
