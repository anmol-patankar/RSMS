namespace Domain.Models;

public partial class TaxRate
{
    public int TaxType { get; set; }

    public int TaxRate1 { get; set; }

    public virtual ICollection<ProductInfo> Products { get; set; } = new List<ProductInfo>();
}