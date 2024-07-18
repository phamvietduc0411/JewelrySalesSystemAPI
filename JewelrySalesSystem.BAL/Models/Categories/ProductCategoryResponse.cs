namespace JewelrySalesSystem.BAL.Models.Categories
{
    public class ProductCategoryResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int PercentPriceRate { get; set; }
        public float ProductionCost { get; set; }
        public bool Status { get; set; }
        public string FeaturedImage { get; set; } = string.Empty;

        // Product Type
        public string ProductType { get; set; } = string.Empty;

        // Gender
        public string Gender { get; set; } = string.Empty;

        // Colour
        public string Colour { get; set; } = string.Empty;
    }
}
