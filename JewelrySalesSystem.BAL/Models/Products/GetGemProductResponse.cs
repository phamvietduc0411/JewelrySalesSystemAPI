namespace JewelrySalesSystem.BAL.Models.Products
{
    public class GetGemProductResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string FeaturedImage { get; set; } = string.Empty;
        public string Shape { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public float Carat { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Clarity { get; set; } = string.Empty;
        public string Cut { get; set; } = string.Empty;
        public float ProductPrice { get; set; }

        public bool IsActive { get; set; }

        public int Quantity { get; set; }

        // Counter 
        public string Counter { get; set; } = string.Empty;

        // Product Type
        public string ProductType { get; set; } = string.Empty;

        // Unit
        public string Unit { get; set; } = string.Empty;
    }
}
