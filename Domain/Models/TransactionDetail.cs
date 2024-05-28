using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class TransactionDetail
{
    public Guid TransactionId { get; set; }

    public string ProductId { get; set; } = null!;

    public int PriceBeforeTax { get; set; }

    public int TaxPercent { get; set; }

    public int DiscountPercent { get; set; }

    public int Quantity { get; set; }

    public virtual ProductInfo Product { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
