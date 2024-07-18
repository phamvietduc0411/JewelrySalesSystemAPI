namespace JewelrySalesSystem.BAL.Models.Materials
{
    public class CreateMaterialRequest
    {
        public string MaterialName { get; set; } = string.Empty;
        public MaterialPrice MaterialPrice { get; set; } = null!;
    }
}
