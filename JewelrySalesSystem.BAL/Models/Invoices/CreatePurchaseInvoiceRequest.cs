namespace JewelrySalesSystem.BAL.Models.Invoices
{
    public class CreatePurchaseInvoiceRequest
    {
        public string InOrOut { get; set; } = string.Empty;

        // Customer
        public string CustomerName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        // User
        public int UserId { get; set; }

        //public float Weight { get; set; }

        //public virtual ICollection<float> Weights {  get; set; } = new List<float>();

        //public float Total { get; set; }

        // Invoice Details
        public virtual ICollection<CreateInvoiceItemRequest> InvoiceDetails { get; set; } = [];
    }
}
