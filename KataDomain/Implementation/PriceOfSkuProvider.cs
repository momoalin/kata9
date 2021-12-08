using System.Collections.Generic;
using Kata.Core.Abstractions;

namespace Kata.Domain.Implementation
{
    public class PriceOfSkuProvider<T> : IPriceOfSkuProvider<T>
    {
        public Dictionary<T, double> Prices { get; set; }

        public PriceOfSkuProvider(Dictionary<T, double> prices)
        {
            Prices = prices;
        }

        public double GetPrice(T sku)
        {
            return Prices[sku];
        }
    }
}
