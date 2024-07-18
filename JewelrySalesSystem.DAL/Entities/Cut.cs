using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("Cut")]
    public class Cut
    {
        [Key]
        public int CutId { get; set; }
        [MaxLength(10)]
        public string Level { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Gems
        public virtual ICollection<Gem> Gems { get; set; } = [];

        // GemPrices
        public virtual ICollection<GemPriceList> GemPrices { get; set; } = [];
    }
}
