using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("Gender")]
    public class Gender
    {
        [Key]
        public int GenderId { get; set; }
        public string GenderName { get; set; } = string.Empty;

        // Products
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
