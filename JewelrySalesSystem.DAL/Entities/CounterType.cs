using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("CounterType")]
    public class CounterType
    {
        public int Id { get; set; }

        [MaxLength(8)]
        public string CounterTypeName { get; set; } = string.Empty;

        public virtual ICollection<Counter> Counters { get; set; } = [];
    }
}
