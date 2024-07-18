using JewelrySalesSystem.BAL.Models.Gems;
using JewelrySalesSystem.BAL.Models.Materials;

namespace JewelrySalesSystem.BAL.Models.Products
{
    public class GetProductResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int PercentPriceRate { get; set; }
        public float ProductionCost { get; set; }
        public string FeaturedImage { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int Quantity { get; set; }
        public float ProductPrice { get; set; }

        // Category
        public string Category { get; set; } = string.Empty;

        // Product Type
        public string ProductType { get; set; } = string.Empty;

        // Gender
        public string Gender { get; set; } = string.Empty;

        // Counter 
        public string Counter { get; set; } = string.Empty;

        // Unit
        public string Unit { get; set; } = string.Empty;

        // Product Gems
        public ICollection<GetGemResponse> Gems { get; set; } = [];

        // Product Materials
        public ICollection<MaterialItem> Materials { get; set; } = [];
    }
}
