using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int PercentPriceRate { get; set; } = 0;
        public float ProductionCost { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public string FeaturedImage { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public float ProductPrice { get; set; } = 0;

        // Unit
        public int? UnitId { get; set; }
        public virtual Unit Unit { get; set; } = null!;

        // Category
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        // Product Type
        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; } = null!;

        // Gender
        public int? GenderId { get; set; }
        public virtual Gender Gender { get; set; } = null!;

        // Counter
        public int? CounterId { get; set; }
        public virtual Counter Counter { get; set; } = null!;

        // Invoice Details
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

        // Product Gems
        public virtual ICollection<ProductGem> ProductGems { get; set; } = new List<ProductGem>();

        // Product Materials
        public virtual ICollection<ProductMaterial> ProductMaterials { get; set; } = new List<ProductMaterial>();
    }
}
