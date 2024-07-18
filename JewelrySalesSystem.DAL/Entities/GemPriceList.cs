using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("GemPriceList")]
    public class GemPriceList
    {
        [Key]
        public int Id { get; set; }
        // Origin
        public int OriginId { get; set; }
        public virtual Origin Origin { get; set; } = null!;

        // Carat
        public int CaratId { get; set; }
        public virtual Carat Carat { get; set; } = null!;

        // Cut
        public int CutId { get; set; }
        public virtual Cut Cut { get; set; } = null!;

        // Clarity
        public int ClarityId { get; set; }
        public virtual Clarity Clarity { get; set; } = null!;

        // Color
        public int ColorId { get; set; }
        public virtual Color Color { get; set; } = null!;

        public float Price { get; set; }

        public DateTime EffDate { get; set; } = DateTime.Now.AddHours(7);
    }
}
