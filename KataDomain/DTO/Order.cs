namespace Kata.Domain.DTO
{
    public class Order<T>
    {
        public T Sku { get; set; }
        public double Price { get; set; }
    }
}