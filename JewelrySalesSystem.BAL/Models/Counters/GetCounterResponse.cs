namespace JewelrySalesSystem.BAL.Models.Counters
{
    public class GetCounterResponse
    {
        public int CounterId { get; set; }
        public string CounterName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string CounterType { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
