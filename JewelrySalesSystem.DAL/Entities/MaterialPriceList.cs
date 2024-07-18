using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("MaterialPriceList")]
    public class MaterialPriceList
    {
        [Key]
        public int Id { get; set; }
        public float BuyPrice { get; set; } 
        public float SellPrice { get; set; }
        public DateTime EffDate { get; set; } = DateTime.Now.AddHours(7);

        // Material
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; } = null!;
    }
}
