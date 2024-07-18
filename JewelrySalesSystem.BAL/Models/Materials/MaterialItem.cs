namespace JewelrySalesSystem.BAL.Models.Materials
{
    public class MaterialItem
    {
        public int MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public float Weight { get; set; }
        public MaterialPrice MaterialPrice { get; set; } = null!;
    }
}
