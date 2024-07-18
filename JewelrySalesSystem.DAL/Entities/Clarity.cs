using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("Clarity")]
    public class Clarity
    {
        [Key]
        public int ClarityId { get; set; }
        [MaxLength(4)]
        public string Level { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Gems
        public virtual ICollection<Gem> Gems { get; set; } = [];

        // GemPrices
        public virtual ICollection<GemPriceList> GemPrices { get; set; } = [];
    }
}
