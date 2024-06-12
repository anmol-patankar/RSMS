namespace Domain.Models;

public partial class TaxRates
{
    public int TaxType { get; set; }

    public int TaxRate { get; set; }

    public virtual ICollection<ProductInfo> ProductInfos { get; set; } = new List<ProductInfo>();
}