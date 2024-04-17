using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class TransactionDetail
{
    public Guid TransactionId { get; set; }

    public Guid ProductId { get; set; }

    public int PriceBeforeTax { get; set; }

    public int TaxAmount { get; set; }

    public int DiscountAmount { get; set; }

    public virtual ProductInfo Product { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
