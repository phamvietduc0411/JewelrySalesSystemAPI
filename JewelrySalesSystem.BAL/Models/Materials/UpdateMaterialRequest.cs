namespace JewelrySalesSystem.BAL.Models.Materials
{
    public class UpdateMaterialRequest
    {
        public int MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
    }
}
