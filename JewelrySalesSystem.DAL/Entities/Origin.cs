using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("Origin")]
    public class Origin
    {
        [Key]
        public int OriginId { get; set; }
        [MaxLength(10)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Gems
        public virtual ICollection<Gem> Gems { get; set; } = [];

        // GemPrices
        public virtual ICollection<GemPriceList> GemPrices { get; set; } = [];
    }
}
