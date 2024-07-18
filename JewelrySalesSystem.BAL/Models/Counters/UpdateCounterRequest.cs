namespace JewelrySalesSystem.BAL.Models.Counters
{
    public class UpdateCounterRequest
    {
        public int CounterId { get; set; }
        public string CounterName { get; set; } = string.Empty;

        public int CounterTypeId { get; set; }
    }
}
