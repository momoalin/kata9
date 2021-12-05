namespace Kata.Domain.DTO
{
    public class PricingRule<T>
    {
        public T SKU { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
    }
}
