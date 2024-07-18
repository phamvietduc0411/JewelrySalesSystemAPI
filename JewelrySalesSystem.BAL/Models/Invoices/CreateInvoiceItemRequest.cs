namespace JewelrySalesSystem.BAL.Models.Invoices
{
    public class CreateInvoiceItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
