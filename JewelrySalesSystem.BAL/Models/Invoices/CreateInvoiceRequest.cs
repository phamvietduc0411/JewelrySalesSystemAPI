namespace JewelrySalesSystem.BAL.Models.Invoices
{
    public class CreateInvoiceRequest
    {
        public string InvoiceStatus { get; set; } = string.Empty;
        public float Total { get; set; }
        public float PerDiscount { get; set; }

        // Customer
        public string CustomerName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        // User
        public int UserId { get; set; }

        // Warranty
        //public int WarrantyId { get; set; }

        // Invoice Details
        public virtual ICollection<CreateInvoiceItemRequest> InvoiceDetails { get; set; } = [];
    }
}
