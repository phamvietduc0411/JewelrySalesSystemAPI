namespace JewelrySalesSystem.BAL.Models.Shapes
{
    public class GetShapeResponse
    {
        public int ShapeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public float PriceRate { get; set; }
    }
}
