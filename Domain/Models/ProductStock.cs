namespace Domain.Models;

public partial class ProductStock
{
    public string ProductId { get; set; } = null!;

    public int StoreId { get; set; }

    public int Quantity { get; set; }

    public int DiscountPercent { get; set; }

    public virtual ProductInfo Product { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}