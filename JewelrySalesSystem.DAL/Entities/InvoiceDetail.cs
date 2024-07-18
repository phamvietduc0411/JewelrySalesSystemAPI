using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("InvoiceDetail")]
    public class InvoiceDetail
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; } = 1;

        public float ProductPrice { get; set; }

        // Product
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        // Invoice
        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; } = null!;
    }
}
