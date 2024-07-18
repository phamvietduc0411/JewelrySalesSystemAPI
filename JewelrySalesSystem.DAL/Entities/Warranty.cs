using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("Warranty")]
    public class Warranty
    {
        [Key]
        public int WarrantyId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Invoices
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
