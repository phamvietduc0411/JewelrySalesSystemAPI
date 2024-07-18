namespace JewelrySalesSystem.BAL.Models.Materials
{
    public class GetMaterialResponse
    {
        public int MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public MaterialPrice MaterialPrice { get; set; } = null!;
    }
}
