using System;
using System.Collections.Generic;
using Kata.Core.Abstractions;

namespace KataDomain.Implementation
{
    public class PriceOfSkuProvider : IPriceOfSkuProvider<char>
    {
        public Dictionary<char, double> Prices { get; set; }

        public PriceOfSkuProvider(Dictionary<char, double> prices)
        {
            Prices = prices;
        }

        public double GetPrice(char sku)
        {
            return Prices[sku];
        }
    }
}
