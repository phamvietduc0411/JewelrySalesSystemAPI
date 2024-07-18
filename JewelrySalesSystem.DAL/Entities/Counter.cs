using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("Counter")]
    public class Counter
    {
        [Key]
        public int CounterId { get; set; }
        public string CounterName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public int? CounterTypeId { get; set; }
        public virtual CounterType CounterType { get; set; } = null!;

        public virtual User? User { get; set; }

        public virtual ICollection<Product> Products { get; set; } = [];
    }
}
