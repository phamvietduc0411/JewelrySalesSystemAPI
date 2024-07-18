using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("ProductMaterial")]
    public class ProductMaterial
    {
        [Key]
        public int Id { get; set; }

        // Product
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        // Material
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; } = null!;

        public float Weight { get; set; } = 0;
    }
}
