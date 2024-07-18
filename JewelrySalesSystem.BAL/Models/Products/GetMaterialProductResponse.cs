using JewelrySalesSystem.BAL.Models.Materials;

namespace JewelrySalesSystem.BAL.Models.Products
{
    public class GetMaterialProductResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public MaterialPrice MaterialPrice { get; set; } = null!;

        // Counter 
        public string Counter { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        // Product Type
        public string ProductType { get; set; } = string.Empty;

        // Unit
        public string Unit { get; set; } = string.Empty;
    }
}
