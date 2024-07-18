namespace JewelrySalesSystem.BAL.Models.Gems
{
    public class GetGemPriceResponse
    {
        public int Id { get; set; }
        public int CaratId { get; set; }
        public int ClarityId { get; set; }
        public int ColorId { get; set; }
        public int CutId { get; set; }
        public int OriginId { get; set; }
        public float Price { get; set; }
        public DateTime EffDate { get; set; }
    }
}
