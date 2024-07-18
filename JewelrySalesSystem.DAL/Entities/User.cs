using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JewelrySalesSystem.DAL.Entities
{
    [Table("User")]
    public class User : IValidatableObject
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Role
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;

        public int? CounterId { get; set; }
        public virtual Counter? Counter { get; set; }

        // Invoices
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
