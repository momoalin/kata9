namespace Kata.Core.Abstractions
{
    public interface IPriceOfSkuProvider<T>
    {
        //give up business rules for discount, processed list
        //set order price according to discount applied
        //whichever rule(by x get n free, by x for n price, buy x per metric) w/conversion
        public double GetPrice(T sku);
    }
}