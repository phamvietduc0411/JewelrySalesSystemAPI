namespace JewelrySalesSystem.BAL.Models.Categories
{
    public class UpdateCategoryRequest
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
