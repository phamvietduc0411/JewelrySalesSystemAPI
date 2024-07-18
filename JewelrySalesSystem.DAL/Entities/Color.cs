using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("Color")]
    public class Color
    {
        [Key]
        public int ColorId { get; set; }
        [MaxLength(3)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Gems
        public virtual ICollection<Gem> Gems { get; set; } = [];

        // GemPrices
        public virtual ICollection<GemPriceList> GemPrices { get; set; } = [];
    }
}
