namespace JewelrySalesSystem.BAL.Models.ProductTypes
{
    public class ProductAndProductTypeResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int PercentPriceRate { get; set; }
        public float ProductionCost { get; set; }
        public string FeaturedImage { get; set; } = string.Empty;

        //Category
        public string Category { get; set; } = string.Empty;

        // Gender
        public string Gender { get; set; } = string.Empty;
    }
}
