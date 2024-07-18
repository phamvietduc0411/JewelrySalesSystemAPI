using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("Carat")]
    public class Carat
    {
        [Key]
        public int CaratId { get; set; }
        public float Weight { get; set; }
        public string Description { get; set; } = string.Empty;

        // Gems
        public virtual ICollection<Gem> Gems { get; set; } = [];

        // GemPrices
        public virtual ICollection<GemPriceList> GemPrices { get; set; } = [];
    }
}
