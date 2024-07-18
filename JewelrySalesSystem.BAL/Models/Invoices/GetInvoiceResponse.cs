namespace JewelrySalesSystem.BAL.Models.Invoices
{
    public class GetInvoiceResponse
    {
        public int InvoiceId { get; set; }
        public DateTime OrderDate { get; set; }
        public string InvoiceType { get; set; } = string.Empty;
        public string InvoiceStatus { get; set; } = string.Empty;
        public float Total { get; set; }
        public float PerDiscount { get; set; }
        public float TotalWithDiscount { get; set; }
        public string InOrOut { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        // Customer
        public string CustomerName { get; set; } = string.Empty;

        // User
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // Warranty
        public string Warranty { get; set; } = string.Empty;

        // Invoice Details
        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }
}
