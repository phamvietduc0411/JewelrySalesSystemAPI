namespace JewelrySalesSystem.BAL.Models.Customers
{
    public class CreateCustomerRequest
    {
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int Point { get; set; }
    }
}
