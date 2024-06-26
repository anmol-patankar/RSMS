﻿namespace RSMS.ViewModels
{
    public class TotalProductInfoModel
    {
        public string ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PriceBeforeTax { get; set; }

        public string Photo { get; set; }

        public int Quantity { get; set; }

        public int DiscountPercent { get; set; }
        public string TaxType { get; set; }
        public int TaxRate { get; set; }
    }
}