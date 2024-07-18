using JewelrySalesSystem.BAL.Models.Materials;

namespace JewelrySalesSystem.BAL.Models.Products
{
    public class CreateProductRequest
    {
        public string ProductName { get; set; } = string.Empty;
        public int PercentPriceRate { get; set; }
        public float ProductionCost { get; set; }
        public string FeaturedImage { get; set; } = string.Empty;
        public int Quantity { get; set; }

        // Category
        public int CategoryId { get; set; }

        // Gender
        public int GenderId { get; set; }

        // Counter
        public int CounterId { get; set; }

        // Product Gems
        public ICollection<int> Gems { get; set; } = [];

        // Product Materials
        public ICollection<CreateMaterialItemRequest> Materials { get; set; } = [];
    }
}
