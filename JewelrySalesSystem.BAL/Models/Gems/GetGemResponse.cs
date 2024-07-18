namespace JewelrySalesSystem.BAL.Models.Gems
{
    public class GetGemResponse
    {
        public int GemId { get; set; }
        public string GemName { get; set; } = string.Empty;
        public string FeaturedImage { get; set; } = string.Empty;
        public string Shape { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public float Carat { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Clarity { get; set; } = string.Empty;
        public string Cut { get; set; } = string.Empty;
        public float Price { get; set; }
        public bool IsActive { get; set; }
    }
}
