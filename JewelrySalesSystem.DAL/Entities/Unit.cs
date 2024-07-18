using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("Unit")]
    public class Unit
    {
        [Key]
        public int UnitId { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Product> Products { get; set; } = [];
    }
}
