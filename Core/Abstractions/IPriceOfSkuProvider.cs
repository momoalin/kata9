namespace Kata.Core.Abstractions
{
    public interface IPriceOfSkuProvider<T>
    {
        public double GetPrice(T sku);
    }
}