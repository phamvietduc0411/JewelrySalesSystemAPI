using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("ProductGem")]
    public class ProductGem
    {
        [Key]
        public int Id { get; set; }      

        // Product
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        // Gem
        public int GemId { get; set; }
        public virtual Gem Gem { get; set; } = null!;
    }
}
