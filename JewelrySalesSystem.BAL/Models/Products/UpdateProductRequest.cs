using JewelrySalesSystem.BAL.Models.Materials;

namespace JewelrySalesSystem.BAL.Models.Products
{
    public class UpdateProductRequest
    {
        public int ProductId { get; set; }
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
        public virtual ICollection<int> Gems { get; set; } = [];

        // Product Materials
        public virtual ICollection<CreateMaterialItemRequest> Materials { get; set; } = [];
    }
}
