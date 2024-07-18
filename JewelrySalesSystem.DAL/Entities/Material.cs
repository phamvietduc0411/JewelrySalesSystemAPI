using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("Material")]
    public class Material
    {
        [Key]
        public int MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Product Materials
        public virtual ICollection<ProductMaterial> ProductMaterials { get; set; } = new List<ProductMaterial>();

        // Material Price List
        public virtual ICollection<MaterialPriceList> MaterialPrices { get; set; } = new List<MaterialPriceList>();
    }
}
