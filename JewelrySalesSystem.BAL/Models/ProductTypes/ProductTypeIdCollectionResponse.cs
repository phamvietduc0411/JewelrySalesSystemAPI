namespace JewelrySalesSystem.BAL.Models.ProductTypes
{
    public class ProductTypeIdCollectionResponse
    {
        public ICollection<ProductAndProductTypeResponse> Products { get; set; } = [];
    }
}
