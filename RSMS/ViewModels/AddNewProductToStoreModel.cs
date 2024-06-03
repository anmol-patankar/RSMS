namespace RSMS.ViewModels
{
    public class AddNewProductToStoreModel
    {
        public int StoreId { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int DiscountPercent { get; set; }
    }
}