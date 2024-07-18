using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("ProductType")]
    public class ProductType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Products
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
