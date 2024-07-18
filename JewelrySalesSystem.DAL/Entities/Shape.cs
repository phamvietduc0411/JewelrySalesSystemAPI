using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("Shape")]
    public class Shape
    {
        [Key]
        public int ShapeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float PriceRate { get; set; }

        // Gems
        public virtual ICollection<Gem> Gems { get; set; } = [];
    }
}
