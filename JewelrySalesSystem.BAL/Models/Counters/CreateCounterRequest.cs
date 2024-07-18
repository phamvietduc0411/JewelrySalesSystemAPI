namespace JewelrySalesSystem.BAL.Models.Counters
{
    public class CreateCounterRequest
    {
        public string CounterName { get; set; } = string.Empty;

        public int CounterTypeId { get; set; }
    }
}
