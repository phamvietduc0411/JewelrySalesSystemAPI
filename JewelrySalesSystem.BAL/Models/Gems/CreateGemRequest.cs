namespace JewelrySalesSystem.BAL.Models.Gems
{
    public class CreateGemRequest
    {
        public string GemName { get; set; } = string.Empty;
        public string FeaturedImage { get; set; } = string.Empty;
        public int ShapeId { get; set; }
        public int OriginId { get; set; }
        public int CaratId { get; set; }
        public int ColorId { get; set; }
        public int ClarityId { get; set; }
        public int CutId { get; set; }
        public float Price { get; set; }
    }
}
