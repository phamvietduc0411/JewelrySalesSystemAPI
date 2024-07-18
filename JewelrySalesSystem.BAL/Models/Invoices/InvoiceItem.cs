namespace JewelrySalesSystem.BAL.Models.Invoices
{
    public class InvoiceItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public string Unit { get; set; } = string.Empty;
        public float ProductPrice { get; set; }
    }
}
