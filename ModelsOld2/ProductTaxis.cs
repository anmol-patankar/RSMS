using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class ProductTaxis
{
    public Guid ProductId { get; set; }

    public int TaxType { get; set; }

    public virtual ProductInfo Product { get; set; } = null!;

    public virtual TaxRate TaxTypeNavigation { get; set; } = null!;
}
