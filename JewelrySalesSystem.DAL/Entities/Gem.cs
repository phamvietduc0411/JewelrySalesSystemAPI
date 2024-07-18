using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("Gem")]
    public class Gem
    {
        [Key]
        public int GemId { get; set; }
        public string GemName { get; set; } = string.Empty;
        public string FeaturedImage { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

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

        // Shape
        public int ShapeId { get; set; }
        public virtual Shape Shape { get; set; } = null!;

        // Product Gems
        public virtual ICollection<ProductGem> ProductGems { get; set; } = new List<ProductGem>();
    }
}
