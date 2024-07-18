namespace JewelrySalesSystem.BAL.Models.Customers
{
    public class GetCustomerResponse
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int Point { get; set; }
        public int Discount { get; set; } = 0;
    }
}
